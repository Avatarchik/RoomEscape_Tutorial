using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace The_A_Drain.Minigames {
		
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
	public class SlidingTile : MonoBehaviour {

		// sliding puzzle 

		RectTransform _rt;
		Image _image;

        Action<SlidingTile> _onTileClicked;

        int startingIndex;
        int currentIndex;

		public int Index {
            get { return currentIndex; }
        }

		public bool IsCorrectIndex {
            get { return currentIndex == startingIndex; }
        }

		public void Initialise(int startIndex, Action<SlidingTile> onClicked)
        {
            startingIndex = startIndex;
            currentIndex = startingIndex;

            _onTileClicked = onClicked;

            _rt = GetComponent<RectTransform>();
            _image = GetComponent<Image>();

            // add the event trigger 
            EventTrigger eTrigger = gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener(OnTileClicked);
            eTrigger.triggers.Add(entry);
        }

        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

		void OnTileClicked(BaseEventData data){
            // respond to the tile being clicked
            
            if (_onTileClicked != null)
            {
                _onTileClicked(this);
            }
		}

        public void MoveToPosition(int newIndex, Vector3 newPosition)
        {
            currentIndex = newIndex;
            if(gameObject.activeSelf)
                StartCoroutine(SlideToPosition(newPosition));
        }

        IEnumerator SlideToPosition(Vector3 newPosition)
        {
            Vector3 startPos = transform.localPosition;
            float duration = 0.1f;
            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Vector3 pos = Vector3.Lerp(startPos, newPosition, elapsed / duration);
                transform.localPosition = pos;
                yield return 0;
            }
            // clamp it
            transform.localPosition = newPosition;
        }
	}
}