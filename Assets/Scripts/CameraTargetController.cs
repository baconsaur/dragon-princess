using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetController : MonoBehaviour {
	public float rotationSpeed;
	private GameObject player;

	void Start () {
		player = GameObject.Find("Player");
	}

	void LateUpdate() {
		transform.position = player.transform.position;

		if (Input.GetAxis("RHorizontal") > 0) {
			transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetAxis("RHorizontal") < 0) {
			transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime, Space.World);
		}
	}
}