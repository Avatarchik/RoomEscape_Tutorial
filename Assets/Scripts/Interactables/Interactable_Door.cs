using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace The_A_Drain.Minigames
{
    public class Interactable_Door : Interactable
    {
        public Interactable[] _requiredCompletedInteractables;

        public Texture2D _openHoverCursor;

        private Image _image;
        public Sprite _open;
        public Sprite _closed;

        public GameObject _currentScreen;
        public GameObject _destinationScreen;

        private bool isOpen = false;

        void Awake()
        {
            base.Awake();
            _image = gameObject.GetComponent<Image>();
            _image.sprite = _closed;
        }

        public override void OnClick(BaseEventData data)
        {
            base.OnClick(data);

            if (!isOpen && CanUse())
            {
                // open the door!
                _image.sprite = _open;
                isOpen = true;

                // ensure the pointer is up to date
                OnPointerEnter(data);
            }
            else if (isOpen)
            {
                _currentScreen.SetActive(false);
                _destinationScreen.SetActive(true);

                // ensure the pointer is reset
                OnPointerExit(data);
            }
            else
            {
                // hmm, it's locked...
                Debug.Log("The door is locked...");
            }
        }

        bool CanUse()
        {
            bool canUse = true;
            foreach (Interactable interactable in _requiredCompletedInteractables)
            {
                if (!interactable.IsActivated())
                {
                    canUse = false;
                }
            }

            return canUse;
        }

        public override void OnPointerEnter(BaseEventData data)
        {
            if (_openHoverCursor && isOpen)
            {
                CursorMode cursorMode = CursorMode.Auto;
                Vector2 hotSpot = Vector2.zero;
                Cursor.SetCursor(_openHoverCursor, hotSpot, cursorMode);
            }
            else
            {
                base.OnPointerEnter(data);
            }
        }

        public override bool IsActivated()
        {
            return CanUse();
        }
    }
}
