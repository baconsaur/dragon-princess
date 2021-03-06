﻿using UnityEngine;
using System.Collections;

public class PlayerTargetController : MonoBehaviour {
	public float moveSpeed;
	public float rotationSpeed;

	private GameObject player;

	void Start() { player = GameObject.Find("Player"); }

	void Update() {
		float vertical = Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		if (vertical == 0 && horizontal == 0) {
			transform.position = Vector3.Lerp(
				transform.position, player.transform.position, Time.deltaTime * moveSpeed);

			return;
		}

		float inputAngle = Mathf.Atan2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")) * Mathf.Rad2Deg;
		transform.eulerAngles = Vector3.Lerp(
			transform.rotation.eulerAngles,
			(Vector3.up * inputAngle) + (Vector3.up * Camera.main.transform.rotation.eulerAngles.y),
			Time.deltaTime * rotationSpeed);

		float inputMagnitude = Mathf.Max(Mathf.Abs(vertical), Mathf.Abs(horizontal));
		transform.Translate(Vector3.forward * inputMagnitude * Time.deltaTime * moveSpeed);
	}
}      