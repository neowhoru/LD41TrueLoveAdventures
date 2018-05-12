using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{

	public Transform Player;


	private void LateUpdate()
	{
		if (Player != null)
		{
			Vector3 newPosition = Player.position;
			newPosition.y = transform.position.y;
			newPosition.x = transform.position.x;
			transform.position = newPosition;	
		}
	}
}
