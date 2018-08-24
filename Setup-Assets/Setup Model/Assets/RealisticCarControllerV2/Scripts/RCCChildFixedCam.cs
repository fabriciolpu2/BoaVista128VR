//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCChildFixedCam : MonoBehaviour {

	[HideInInspector]public Transform player;
	public float distance = 50f;

	void Update () {

		if(!player)
			return;

		transform.LookAt(new Vector3(player.position.x, player.position.y + 0f, player.position.z));

	}

}
