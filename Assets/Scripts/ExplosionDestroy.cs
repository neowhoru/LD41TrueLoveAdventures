using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroy : MonoBehaviour {


	public void DestroyMe()
	{
		Debug.Log("Destroy Explosion");
		Destroy(gameObject);
	}
}
