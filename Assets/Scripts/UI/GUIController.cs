using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	private GUIDialogueRenderer guiDialogueRenderer;
	private List<GameObject> dialogueObjects;
	private GameObject activeNotification;

	private bool active;
	private float cooldown;
	private NPCController target;

	void Start() {
		active = false;
		dialogueObjects = new List<GameObject>();
		guiDialogueRenderer = GetComponent<GUIDialogueRenderer>();
	}

	void Update() {
		cooldown -= Time.deltaTime;
		if (cooldown > 0 || !active) return;

		if (Input.GetButton("Fire1")) {
			SelectNode();
			cooldown = 0.3f;
		} else if (Input.GetAxis("Vertical") != 0) {
			guiDialogueRenderer.MoveCursor(dialogueObjects);
			cooldown = 0.3f;
		}
	}

	public bool isActive() { return active; }

	public void StartDialogue(NPCController npc) {
		this.target = npc;

		dialogueObjects.AddRange(guiDialogueRenderer.SetDialogue(target));
		guiDialogueRenderer.MoveCursor(dialogueObjects);

		cooldown = 0.3f;
		active = true;
	}

	private void SelectNode() {
		List<GameObject> newDialogue = guiDialogueRenderer.SelectNode(target);
		if (newDialogue != null) {
			DestroyText();
			if (newDialogue.Count == 0) {
				active = false;
				target = null;
				return;
			}
			dialogueObjects.AddRange(newDialogue);
		}
	}

	private void DestroyText() {
		foreach (GameObject textObject in dialogueObjects) {
			Destroy(textObject);
		}
		dialogueObjects = new List<GameObject>();
	}
}