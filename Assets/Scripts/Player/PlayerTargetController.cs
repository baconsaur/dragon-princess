using UnityEngine;
using System.Collections;

public class PlayerTargetController : MonoBehaviour {
	public float moveSpeed;
	public float rotationSpeed;
	public bool isRotationFlipped;

	private GameObject player;

	void Start() {
		player = GameObject.Find("Player");
		isRotationFlipped = false;
	}

	void Update() {
		if (Input.GetAxis("Vertical") != 0) {
			transform.Translate(Vector3.forward * Mathf.Abs(Input.GetAxis("Vertical")) * Time.deltaTime * moveSpeed);

			if (Input.GetAxis("Vertical") < 0 && !isRotationFlipped) {
				transform.RotateAround(player.transform.position, Vector3.up, 180);
				isRotationFlipped = true;
			}
		} else {
			isRotationFlipped = false;
			transform.position = player.transform.position;
		}

		if (Input.GetAxis("Horizontal") != 0) {
			transform.RotateAround(
				player.transform.position,
				Vector3.up,
				Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed);
		}
	}
}