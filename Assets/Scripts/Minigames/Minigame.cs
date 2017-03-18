using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace The_A_Drain.Minigames {

	public class Minigame : MonoBehaviour {

        protected System.Action _onComplete;

		[SerializeField]
		protected bool _isComplete = false;
		public virtual bool IsComplete { get { return _isComplete; }}

        public void SetOnCompleteCallback(System.Action callback)
        {
            _onComplete = callback;
        }

        protected void SetComplete()
        {
            _isComplete = true;
            if (_onComplete != null)
            {
                _onComplete();
            }
        }
    }

	public class GridBasedMinigame : Minigame {

		// encapsulate some helper functions both of our minigames will use
		protected int _gridWidth = 3;
		protected int _gridHeight = 3;
		protected float _tileWidth;
		protected float _tileHeight;

		// ui canvas
		protected RectTransform _canvas;
		protected float _canvasWidth;
		protected float _canvasHeight;
		protected float _tileSpacing = 5f;

		protected int IndexForGridPosition(int xPos, int yPos){
			return (_gridWidth * yPos) + xPos;
		}

		protected Vector2 GridPositionForTileIndex(int index)
		{
			int x = index % _gridWidth;
			int y = index / _gridWidth;
			Debug.Log("GridPos: [" + x + "," + y + "] for index: " + index);
			return new Vector2(index % _gridWidth, index / _gridHeight);
		}

		protected Vector3 PositionForTile(int tileX, int tileY)
		{
			float tileXPos = (_tileWidth * tileX) + ((_tileWidth / 2) - _canvas.sizeDelta.x / 2);
			float tileYPos = (_tileHeight * tileY) + ((_tileHeight / 2) - _canvas.sizeDelta.y / 2);

			Vector3 position = new Vector3(tileXPos, tileYPos, 0);
			return position;
		}

		protected  Vector3 PositionForTileIndex(int index)
		{
			Vector2 gridPosition = GridPositionForTileIndex(index);
			return PositionForTile((int)gridPosition.x, (int)gridPosition.y);
		}
	}
}