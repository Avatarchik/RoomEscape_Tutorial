using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFitTest : MonoBehaviour {

    public GameObject blockFitPuzzle;
    public RectTransform _Rt;
    public RectTransform _pieceStoreRT;

	// Use this for initialization
	void Awake () {

        The_A_Drain.Minigames.BlockFitPuzzle.CreatePuzzleWithParent(_Rt, _pieceStoreRT, blockFitPuzzle);	
	}
}
