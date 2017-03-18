using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	public GameObject dialoguePrefab;
	public GameObject responsePrefab;

	private List<GameObject> visibleText;
	private bool active;
	private DialogueNode[] currentDialogue;
	private int dialogueIndex;
	private int responseIndex;
	private float cooldown;
	private string activeText;

	void Start() {
		active = false;
		visibleText = new List<GameObject>();
	}

	void Update() {
		cooldown -= Time.deltaTime;
		if (cooldown > 0 || !active) return;

		if (Input.anyKeyDown) {
			MoveCursor();
			PerformAction();
			cooldown = 0.3f;
		}
	}

	public bool isActive() { return active; }

	public void StartDialogue(DialogueNode[] dialogue) {
		currentDialogue = dialogue;
		dialogueIndex = 0;
		responseIndex = 0;
		cooldown = 0.3f;
		active = true;

		SelectNode();
	}

	private void PerformAction() {
		if (Input.GetButtonDown("Fire1")) {
			DestroyText();

			Action[] actions = currentDialogue[dialogueIndex].actions;
			if (actions.Length > 0 && actions[responseIndex].nextNode > 0) {
				dialogueIndex = actions[responseIndex].nextNode;
				SelectNode();
			} else if (actions.Length == 0 && dialogueIndex < currentDialogue.Length - 1) {
				dialogueIndex++;
				SelectNode();
			} else {
				active = false;
				DestroyText();
			}
		}
	}

	private void MoveCursor() {
		if (currentDialogue[dialogueIndex].actions.Length > 0) {
			visibleText[responseIndex].GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
			if (Input.GetAxis("Horizontal") > 0) {
				responseIndex += 1;
			} else if (Input.GetAxis("Horizontal") < 0) {
				responseIndex -= 1;
			} else if (Input.GetAxis("Vertical") > 0) {
				responseIndex -= 2;
			} else if (Input.GetAxis("Vertical") < 0) {
				responseIndex += 2;
			}

			int max = visibleText.Count - 2 > 0 ? visibleText.Count - 2 : 0;

			responseIndex = Mathf.Clamp(responseIndex, 0, max);

			visibleText[responseIndex].GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
		}
	}

	private void SelectNode() {
		responseIndex = 0;

		if (currentDialogue[dialogueIndex].actions.Length > 0) {
			int i = 1;
			foreach (var action in currentDialogue[dialogueIndex].actions) {
				GameObject responseContainer = CreateTextContainer(responsePrefab, i);
				responseContainer.GetComponentInChildren<Text>().text = action.text;
				i++;
			}
		}

		StartCoroutine(ScrollText(currentDialogue[dialogueIndex].text, CreateTextContainer(dialoguePrefab, 0)));

		MoveCursor();
	}

	private GameObject CreateTextContainer(GameObject prefab, int index) {
		RectTransform rectTransform = prefab.GetComponent<RectTransform>();
		float xOffset = 0;
		float yOffset = 0;

		if (index > 0) {
			RectTransform dialogueTransform = dialoguePrefab.GetComponent<RectTransform>();
			xOffset = index == 2 || index == 4 ? dialogueTransform.rect.width - rectTransform.rect.width : 0;
			yOffset = index == 3 || index == 4 ? -rectTransform.rect.height : 0;
			yOffset -= dialogueTransform.rect.height;
		}

		Vector3 position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
		GameObject textContainer = Instantiate(prefab, position, Quaternion.identity);
		textContainer.transform.Translate(prefab.transform.position);
		textContainer.transform.SetParent(transform);

		visibleText.Add(textContainer);
		return textContainer;
	}

	private void DestroyText() {
		StopAllCoroutines();
		foreach (GameObject textObject in visibleText) {
			Destroy(textObject);
		}
		visibleText = new List<GameObject>();
	}

	private IEnumerator ScrollText(string text, GameObject container) {
		if (container != null) {
			Text content = container.GetComponentInChildren<Text>();
			for (int l = 0; l < text.Length; l += 1) {
				content.text = content.text + text.Substring(l, 1);
				yield return new WaitForSeconds(0.05f);
			}
		}
	}
}