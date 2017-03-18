using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace The_A_Drain.Minigames
{
    [RequireComponent(typeof(RectTransform))]
    public class BlockFitPiece : MonoBehaviour
    {
        RectTransform _rt;
        bool isAnchoredToPointer = false;

        bool isPlacedCorrectly = false;
        public bool IsPlacedCorrectly { get { return isPlacedCorrectly; } }

        Action _onWasPlacedCorrectly;
        Vector3 _correctPosition;
        Vector3 _storedPosition;
        float _distanceTolerance;

        // Use this for initialization
        void Awake()
        {
            _rt = gameObject.GetComponent<RectTransform>();

            // add the event triggers
            EventTrigger eTrigger = gameObject.AddComponent<EventTrigger>();

            // pointer down event
            EventTrigger.Entry downEvent = new EventTrigger.Entry();
            downEvent.eventID = EventTriggerType.PointerDown;
            downEvent.callback.AddListener(OnPointerDown);
            eTrigger.triggers.Add(downEvent);

            // point up event
            EventTrigger.Entry upEvent = new EventTrigger.Entry();
            upEvent.eventID = EventTriggerType.PointerUp;
            upEvent.callback.AddListener(OnPointerUp);
            eTrigger.triggers.Add(upEvent);
        }

        private void Update()
        {
            if (isAnchoredToPointer)
            {
                // find the pointers current position and move there
                Vector2 pos;
                pos = Input.mousePosition;
                _rt.position = pos;
            }
        }

        public void SetCorrectPosition()
        {
            _correctPosition = transform.localPosition;
        }

        public void SetStoredPosition()
        {
            _storedPosition = transform.localPosition;
        }

        public void SetOnPlacedCallback(Action onPlaced)
        {
            _onWasPlacedCorrectly = onPlaced;
        }

        public void SetDistanceTolerance(float tolerance)
        {
            _distanceTolerance = tolerance;
        }

        private void OnPointerDown(BaseEventData data)
        {
            isAnchoredToPointer = true;
        }

        private void OnPointerUp(BaseEventData data)
        {
            isAnchoredToPointer = false;
            if (IsWithinDistanceOfStartingPosition())
            {
                // correctly placed
                isPlacedCorrectly = true;
                transform.localPosition = _correctPosition;

                // finally disable it
                enabled = false;

                if(_onWasPlacedCorrectly != null)
                {
                    _onWasPlacedCorrectly();
                }
            } else
            {
                // snap back to the stored position
                //transform.localPosition = _storedPosition;
            }
        }

        public bool IsWithinDistanceOfStartingPosition()
        {
            float distance = Vector3.Distance(_correctPosition, transform.localPosition);
            return distance <= _distanceTolerance;
        }
    }
}