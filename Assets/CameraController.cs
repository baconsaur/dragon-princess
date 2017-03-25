using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject player;
	public float rotationSpeed;

	private Vector3 playerOffset;

	void Start ()
	{
		playerOffset = transform.position - player.transform.position;
	}

	void Update() {
		if (Input.GetAxis("RHorizontal") > 0) {
			transform.RotateAround(player.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
		}
		if (Input.GetAxis("RHorizontal") < 0) {
			transform.RotateAround(player.transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
		}

		playerOffset = transform.position - player.transform.position;
	}

	void LateUpdate () {
		transform.position = player.transform.position + playerOffset;
	}
}
