using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace The_A_Drain.Minigames
{
    public class Interactable_StartSlidingPuzzle : Interactable
    {
        public RectTransform _slidePuzzleScreen;
        public RectTransform _slidePuzzleContainer;
        public Texture2D _slidePuzzleImage;

        private SlidingPuzzle _slidingPuzzle = null;

        public override void OnClick(BaseEventData data)
        {
            base.OnClick(data);

            _slidePuzzleScreen.gameObject.SetActive(true);

            if (_slidingPuzzle == null)
            {
                _slidingPuzzle = SlidingPuzzle.CreatePuzzleWithParent(_slidePuzzleContainer, _slidePuzzleImage);
                _slidingPuzzle.SetOnCompleteCallback(OnCompletePuzzle);
            } 
        }

        void OnCompletePuzzle()
        {
            OnExitPuzzle();
        }

        void OnExitPuzzle()
        {
            _slidePuzzleScreen.gameObject.SetActive(false);
        }

        public override bool IsActivated()
        {
            return _slidingPuzzle != null && _slidingPuzzle.IsComplete;
        }
    }
}
