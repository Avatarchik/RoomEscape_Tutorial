using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace The_A_Drain.Minigames
{
    public class Interactable_StartBlockFitPuzzle : Interactable
    {
        public RectTransform _blockPuzzleScreen;
        public RectTransform _blockPuzzleContainer;
        public RectTransform _blockFitPieceStoreContainer;
        public GameObject _blockPuzzlePrefab;

        private BlockFitPuzzle _blockFitPuzzle = null;

        public override void OnClick(BaseEventData data)
        {
            base.OnClick(data);

            _blockPuzzleScreen.gameObject.SetActive(true);

            if (_blockFitPuzzle == null)
            {
                _blockFitPuzzle = BlockFitPuzzle.CreatePuzzleWithParent(
                    _blockPuzzleContainer, 
                    _blockFitPieceStoreContainer, 
                    _blockPuzzlePrefab);

                _blockFitPuzzle.SetOnCompleteCallback(OnCompletePuzzle);
            } 
        }

        void OnCompletePuzzle()
        {
            OnExitPuzzle();
        }

        void OnExitPuzzle()
        {
            _blockPuzzleScreen.gameObject.SetActive(false);
        }

        public override bool IsActivated()
        {
            return _blockFitPuzzle != null && _blockFitPuzzle.IsComplete;
        } 
    }
}
