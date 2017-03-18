using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour {

    public Texture2D hoverCursor;

	public virtual void Awake () {
        EventTrigger eTrigger = gameObject.AddComponent<EventTrigger>();
		EventTrigger.Entry pointerClick = new EventTrigger.Entry();
        pointerClick.eventID = EventTriggerType.PointerClick;
        pointerClick.callback.AddListener(OnClick);
		eTrigger.triggers.Add(pointerClick);
        
        // cursor handling
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener(OnPointerEnter);
        eTrigger.triggers.Add(pointerEnter);

        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener(OnPointerExit);
        eTrigger.triggers.Add(pointerExit);
    }

	public virtual void OnClick(BaseEventData data){
		Debug.Log("We clicked an interactable!");
	}

    public virtual void OnPointerEnter(BaseEventData data)
    {
        if (hoverCursor) {
            CursorMode cursorMode = CursorMode.Auto;
            Vector2 hotSpot = Vector2.zero;
            Cursor.SetCursor(hoverCursor, hotSpot, cursorMode);
        }
    }

    public virtual void OnPointerExit(BaseEventData data)
    {
        CursorMode cursorMode = CursorMode.Auto;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    public virtual bool IsActivated()
    {
        return true;
    }
}
