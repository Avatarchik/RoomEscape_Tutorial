using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PositionHelper : MonoBehaviour {
	
	[MenuItem("Scene Tools/Align Sprites")]
	public static void AlignSceneSprites(){
		GameObject canvas = GameObject.Find("Canvas");
		Transform canvasTransform = canvas.transform;

		CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
		Vector2 referenceResolution = scaler.referenceResolution;

		Image[] images = canvasTransform.GetComponentsInChildren<Image>();

		foreach(Image img in images){

			Sprite sprite = img.sprite;

			if(sprite != null){
				Rect spriteRect = sprite.rect;
				Vector2 position = spriteRect.center;
				Vector2 offset = referenceResolution/2;

				img.transform.localPosition = position - offset;
			}
		}
	}
}
