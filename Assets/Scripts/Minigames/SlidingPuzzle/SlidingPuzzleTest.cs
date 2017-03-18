using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPuzzleTest : MonoBehaviour {

	public Texture2D image;

	// Use this for initialization
	void Start () {
		RectTransform rt = gameObject.GetComponent<RectTransform>();
		The_A_Drain.Minigames.SlidingPuzzle.CreatePuzzleWithParent(rt, image);
	}
}
