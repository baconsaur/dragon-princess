using UnityEngine;
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
			transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.8f);
			return;
		}

		Vector3 cameraDirection = Camera.main.transform.rotation.eulerAngles;
		Quaternion newRotation = Quaternion.identity;

		if (vertical > 0) {
			newRotation = Quaternion.Euler(new Vector3(0, cameraDirection.y, 0));
		} else if (vertical < 0) {
			newRotation = Quaternion.Euler(new Vector3(0, cameraDirection.y + 180, 0));
		} else if (horizontal > 0) {
			newRotation = Quaternion.Euler(new Vector3(0, cameraDirection.y + 90, 0));
		} else if (horizontal < 0) {
			newRotation = Quaternion.Euler(new Vector3(0, cameraDirection.y + 270, 0));
		}

		transform.rotation = newRotation;
		float inputMagnitude = Mathf.Max(Mathf.Abs(vertical), Mathf.Abs(horizontal));
		transform.Translate(Vector3.forward * inputMagnitude * Time.deltaTime * moveSpeed);
	}
}