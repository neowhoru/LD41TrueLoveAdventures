using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShakeSimple : MonoBehaviour {

	Vector3 originalCameraPosition;
	public float shakeAmt = 0.015f;
	public Camera mainCamera;
	
	void Start()
	{
		mainCamera = Camera.main;
	}


	private void OnCollisionEnter2D(Collision2D other)
	{
		originalCameraPosition = mainCamera.transform.position;
		Debug.Log("COLLIDER 2D SHAKER");
		//shakeAmt = coll.relativeVelocity.magnitude * .0025f;
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", 0.3f);

	}

	public void ExecuteManualCameraShake()
	{
		originalCameraPosition = mainCamera.transform.position;
		Debug.Log("COLLIDER 2D SHAKER");
		//shakeAmt = coll.relativeVelocity.magnitude * .0025f;
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", 0.3f);
	}

	void CameraShake()
	{
		if (shakeAmt > 0)
		{
			float quakeAmt = Random.value * shakeAmt * 2 - shakeAmt;
			Vector3 pp = mainCamera.transform.position;
			pp.y += quakeAmt; // can also add to x and/or z
			mainCamera.transform.position = pp;
		}
	}

	void StopShaking()
	{
		CancelInvoke("CameraShake");
		mainCamera.transform.position = originalCameraPosition;
	}
	
}
