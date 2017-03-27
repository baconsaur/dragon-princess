using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float moveSpeed;
	public float rotationSpeed;

	private GUIController GuiController;
	private Animator animator;
	private GameObject target;

	void Start() {
		GuiController = GameObject.Find("Canvas").GetComponent<GUIController>();
		animator = GetComponent<Animator>();
		target = GameObject.Find("Player Target");
	}

	void LateUpdate() {
		if (GuiController.isActive()) return;

		UpdatePlayer();
	}

	public void OnTriggerStay(Collider other) {
		if (!GuiController.isActive() && other.CompareTag("NPC") && Input.GetButtonDown("Fire1")) {
			NPCController npcController = other.GetComponent<NPCController>();
			GuiController.StartDialogue(npcController);
		}
	}

	private void UpdatePlayer() {
		float inputMagnitude = Mathf.Max(
			Mathf.Abs(Input.GetAxis("Vertical")),
			Mathf.Abs(Input.GetAxis("Horizontal")));

		bool shouldWalk = transform.position != target.transform.position;
		bool shouldRun = inputMagnitude > 0.7f;

		if (animator.GetBool("walking") != shouldWalk) {
			animator.SetBool("walking", shouldWalk);
		} else if (animator.GetBool("running") != shouldRun) {
			animator.SetBool("running", shouldRun);
		}

		transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
		transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, Time.deltaTime * rotationSpeed);
	}
}