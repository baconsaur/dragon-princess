using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject target;
	public float rotationSpeed;

	void Update() {
		if (Input.GetAxis("RHorizontal") > 0) {
			transform.RotateAround(target.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
		}
		if (Input.GetAxis("RHorizontal") < 0) {
			transform.RotateAround(target.transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);
		}
	}
}
