using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace The_A_Drain.Minigames {
		
	public class SlidingPuzzle : GridBasedMinigame {

		// game status
		SlidingTile[,] _gameBoard;
		SlidingTile _currentOpenTile;

		// game setup
		Texture2D _boardImage;

		public static SlidingPuzzle CreatePuzzleWithParent(RectTransform parent, Texture2D puzzleImage){

			// create the puzzle canvas
			GameObject puzzleCanvas = new GameObject("SlidingPuzzle");
            RectTransform canvas = puzzleCanvas.AddComponent<RectTransform>();
            canvas.SetParent(parent);
            canvas.localPosition = Vector3.zero;
            canvas.localScale = Vector3.one;
            canvas.sizeDelta = parent.sizeDelta;

            SlidingPuzzle slidingPuzzle = puzzleCanvas.AddComponent<SlidingPuzzle>();

			slidingPuzzle.CreatePuzzleBoard(canvas, puzzleImage);

            return slidingPuzzle;
		}

		void CreatePuzzleBoard(RectTransform parent, Texture2D puzzleImage)
        {
            _canvas = parent;
            _boardImage = puzzleImage;

            _tileWidth = (_canvas.sizeDelta.x / _gridWidth);
            _tileHeight = (_canvas.sizeDelta.y / _gridHeight);

            _gameBoard = new SlidingTile[_gridWidth, _gridHeight];
            
			for(int y = 0; y < _gridHeight; y++){
				for (int x = 0; x < _gridWidth; x++){
					
					CreateTile(x, y);
					SetTileSprite(x, y);
				}
			}

            _currentOpenTile = _gameBoard[0,0];
            _currentOpenTile.gameObject.SetActive(false);

			StartCoroutine(RandomizeInitialLayout());
		}

		void CreateTile(int tileX, int tileY){

			int tileIndex = IndexForGridPosition(tileX, tileY);
			GameObject tile = new GameObject("Tile_" + tileIndex);
			RectTransform tileRT = tile.AddComponent<RectTransform>();
			Image tileImage = tile.AddComponent<Image>();

			tile.transform.SetParent(_canvas);
            tile.transform.localScale = Vector3.one;

            tileRT.sizeDelta = new Vector2(_tileWidth - _tileSpacing, _tileHeight - _tileSpacing);

            tile.transform.localPosition = PositionForTile(tileX, tileY);

            _gameBoard[tileX, tileY] = tile.AddComponent<SlidingTile>();
            _gameBoard[tileX, tileY].Initialise(tileIndex, OnTileClicked);
		}        

		void SetTileSprite(int tileX, int tileY){

            float spriteWidth = _boardImage.width / _gridWidth;
            float spriteHeight = _boardImage.height / _gridHeight;

            Rect spriteRect = new Rect(spriteWidth * tileX, spriteHeight * tileY, spriteWidth, spriteHeight);
            Vector2 pivot = new Vector2(0.5f, 0.5f);

            Sprite sprite = Sprite.Create(_boardImage, spriteRect, pivot);
            _gameBoard[tileX, tileY].SetSprite(sprite);
        }

        void OnTileClicked(SlidingTile clickedTile)
        {
            if (_isComplete)
            {
                return;
            }

            Vector2 gridPos = GridPositionForTileIndex(clickedTile.Index);
            int tileX = (int)gridPos.x;
            int tileY = (int)gridPos.y;

            // poll all four directions for an opent spot
            bool left = CanMoveLeft(tileX, tileY);
            bool right = CanMoveRight(tileX, tileY);
            bool up = CanMoveUp(tileX, tileY);
            bool down = CanMoveDown(tileX, tileY);

            bool canMoveToOpenSpace = (left || right || up || down);

            if (canMoveToOpenSpace)
            {
                SwapTileWithOpenSpace(clickedTile);
                CheckComplete();
            }
        }

        bool CanMoveLeft(int tileX, int tileY)
        {
            return (tileX - 1) >= 0 && _gameBoard[tileX - 1, tileY] == _currentOpenTile;
        }

        bool CanMoveRight(int tileX, int tileY)
        {
            return (tileX + 1) < _gridWidth && _gameBoard[tileX + 1, tileY] == _currentOpenTile;
        }

        bool CanMoveUp(int tileX, int tileY)
        {
            return (tileY + 1) < _gridHeight && _gameBoard[tileX, tileY + 1] == _currentOpenTile;
        }

        bool CanMoveDown(int tileX, int tileY)
        {
            return (tileY - 1) >= 0 && _gameBoard[tileX, tileY - 1] == _currentOpenTile;
        }

        void SwapTileWithOpenSpace(SlidingTile clickedTile)
        {
            int newOpenTileIndex = clickedTile.Index;
            int newTileIndex = _currentOpenTile.Index;
            Vector2 newOpenGridPos = GridPositionForTileIndex(newOpenTileIndex);
            Vector3 newTileGridPos = GridPositionForTileIndex(newTileIndex);

            _gameBoard[(int)newOpenGridPos.x, (int)newOpenGridPos.y] = _currentOpenTile;
            _gameBoard[(int)newTileGridPos.x, (int)newTileGridPos.y] = clickedTile;

            Vector3 newOpenTilePosition = PositionForTileIndex(newOpenTileIndex);
            Vector3 newTilePosition = PositionForTileIndex(newTileIndex);

            clickedTile.MoveToPosition(newTileIndex, newTilePosition);
            _currentOpenTile.MoveToPosition(newOpenTileIndex, newOpenTilePosition);
        }

		IEnumerator RandomizeInitialLayout()
		{
			int steps = 99;

			for(int i = steps; steps > 0; steps--){

				Vector2 openGridPos = GridPositionForTileIndex(_currentOpenTile.Index);
				int targetX = (int)openGridPos.x;
				int targetY = (int)openGridPos.y;

				if(Random.Range(0, 100) > 50){
					targetX = Random.Range(targetX - 1, targetX + 2);
					Debug.Log("rand: " + targetX);
					targetX = Mathf.Clamp(targetX, 0, _gridWidth-1);
					Debug.Log("clamped: " + targetX);
				}
				else
				{
					targetY = Random.Range(targetY - 1, targetY + 2);
					targetY = Mathf.Clamp(targetY, 0, _gridHeight-1);
				}

				// if it's a different tile, swap them
				if(targetX != (int)openGridPos.x || targetY != (int)openGridPos.y){
					// swap them!
					Debug.Log("SWAPPING...");
					SwapTileWithOpenSpace(_gameBoard[targetX, targetY]);
					yield return new WaitForSeconds(0.105f);
				} else {
					i++; // we didn't actually move, so don't count a step
				}
			}
		}

        void CheckComplete()
        {
            bool correct = true;
            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    if (!_gameBoard[x, y].IsCorrectIndex)
                    {
                        correct = false;
                        break;
                    }
                }
            }

            if (correct)
            {
                SetComplete();
                _currentOpenTile.gameObject.SetActive(true);
            }
        }
    }
}