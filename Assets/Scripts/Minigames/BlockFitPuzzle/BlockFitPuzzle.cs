using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace The_A_Drain.Minigames {

	public class BlockFitPuzzle : GridBasedMinigame {

		RectTransform _pieceStoreCanvas; // for the unplaced pieces to start/return to

        List<BlockFitPiece> _pieces = new List<BlockFitPiece>();

        float _placeDistanceTolerance = 30.0f;

		public static BlockFitPuzzle CreatePuzzleWithParent(RectTransform boardParent, RectTransform pieceStoreParent, GameObject puzzlePrefab){

            // create the piece store canvas
            GameObject pieceCanvasObject = new GameObject("PieceStore");
            RectTransform pieceCanvas = pieceCanvasObject.AddComponent<RectTransform>();
            pieceCanvas.SetParent(pieceStoreParent);
            pieceCanvas.localPosition = Vector3.zero;
            pieceCanvas.localScale = Vector3.one;
            pieceCanvas.sizeDelta = pieceStoreParent.sizeDelta;

            // create the puzzle canvas and pieces
            GameObject puzzleCanvas = Instantiate(puzzlePrefab);
			RectTransform canvas = puzzleCanvas.GetComponent<RectTransform>();
			canvas.SetParent(boardParent);
			canvas.localPosition = Vector3.zero;
			canvas.localScale = Vector3.one;
			canvas.sizeDelta = boardParent.sizeDelta;

			BlockFitPuzzle blockFitPuzzle = puzzleCanvas.AddComponent<BlockFitPuzzle>();
            blockFitPuzzle.CreatePuzzleBoard(canvas, pieceCanvas);

            return blockFitPuzzle;
        }

		public void CreatePuzzleBoard(RectTransform boardCanvas, RectTransform pieceStoreCanvas){

			_canvas = boardCanvas;
			_pieceStoreCanvas = pieceStoreCanvas;

            // get all children on the board canvas, these are our actual pieces
            foreach(Transform t in _canvas)
            {
                BlockFitPiece piece = t.gameObject.AddComponent<BlockFitPiece>();
                piece.SetOnPlacedCallback(OnPiecePlaced);
                piece.SetDistanceTolerance(_placeDistanceTolerance);
                _pieces.Add(piece);
            }

            StartCoroutine(SetPieceStartingPositions());
		}

        public IEnumerator SetPieceStartingPositions()
        {
            yield return 0;
            // place them randomly within the piece store location

            float inset = 40f;
            float max_x = (_pieceStoreCanvas.sizeDelta.x / 2) - inset;
            float max_y = (_pieceStoreCanvas.sizeDelta.y / 2) - inset;

            foreach (BlockFitPiece piece in _pieces)
            {
                // set the correct position
                piece.SetCorrectPosition();
                // parent it to the piece store temporarily to work out the position a bit easier
                piece.transform.SetParent(_pieceStoreCanvas);
                piece.transform.localPosition = new Vector3()
                {
                    x = Random.Range(-max_x, max_x),
                    y = Random.Range(max_y, -max_y),
                    z = 0.0f
                };
                // parent it to the puzzle canvas again so it'll set the correct position
                piece.transform.SetParent(_canvas);
                // set the stored position
                piece.SetStoredPosition();
            }
        }

        public void OnPiecePlaced()
        {
            // check if it's within the target distance
            // if it is, mark it as being completed
            CheckComplete();
        }

        void CheckComplete()
        {
            bool correct = true;
            foreach(BlockFitPiece piece in _pieces)
            {
                if (!piece.IsPlacedCorrectly)
                {
                    correct = false;
                }
            }

            if (correct)
            {
                SetInputEnabled(false);
                SetComplete();
            }
        }

        void SetInputEnabled(bool isEnabled)
        {
            foreach(BlockFitPiece piece in _pieces)
            {
                piece.enabled = false;
            }
        }

    }
}
