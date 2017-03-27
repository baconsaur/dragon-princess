using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetController : MonoBehaviour {
	public float analogRotationSpeed;
	public float mouseRotationSpeed;
	private GameObject player;

	void Start () {
		player = GameObject.Find("Player");
	}

	void LateUpdate() {
		transform.position = player.transform.position;

		float actualRotationSpeed = Input.GetAxis("Mouse X") != 0 ? mouseRotationSpeed : analogRotationSpeed;

		if (Input.GetAxis("RHorizontal") > 0 || Input.GetAxis("Mouse X") > 0) {
			transform.Rotate(Vector3.up * actualRotationSpeed * Time.deltaTime, Space.World);
		}
		if (Input.GetAxis("RHorizontal") < 0 || Input.GetAxis("Mouse X") < 0) {
			transform.Rotate(Vector3.up * -actualRotationSpeed * Time.deltaTime, Space.World);
		}
	}
}