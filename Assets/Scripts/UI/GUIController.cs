using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	public GameObject dialoguePrefab;
	public GameObject responsePrefab;

	private DialogueManager dialogueManager;
	private List<GameObject> visibleText;
	private bool active;
	private float cooldown;
	private string fullDialogue;
	private NPCController target;

	void Start() {
		active = false;
		visibleText = new List<GameObject>();
		dialogueManager = new DialogueManager();
	}

	void Update() {
		cooldown -= Time.deltaTime;
		if (cooldown > 0 || !active) return;

		if (Input.GetButton("Fire1") || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) {
			MoveCursor();
			PerformAction();
			cooldown = 0.3f;
		}
	}

	public bool isActive() { return active; }

	public void StartDialogue(NPCController npc) {
		this.target = npc;
		dialogueManager.SetCurrentDialogue(npc.GetDialogue());

		cooldown = 0.3f;
		active = true;

		CreateText();
	}

	private void PerformAction() {
		if (Input.GetButtonDown("Fire1")) {
			if (fullDialogue != null) {
				StopAllCoroutines();
				visibleText[visibleText.Count - 1]
					.GetComponentInChildren<Text>()
					.text = fullDialogue;
				fullDialogue = null;
				return;
			}
			SelectNode();
		}
	}

	private void EndConversation() { }

	private void SelectNode() {
		if (!dialogueManager.isAllowed(target.GetAffection())) return;

		int consequence = dialogueManager.GetResult();
		if (consequence != 0) {
			target.UpdateAffection(consequence);
		}

		DestroyText();
		if (dialogueManager.GetNextNode() < 0 || !dialogueManager.HasActions()) {
			active = false;
			target = null;
			return;
		}

		dialogueManager.SelectNode();
		CreateText();
	}

	private void MoveCursor() {
		visibleText[dialogueManager.GetCursor()].GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
		visibleText[dialogueManager.MoveCursor()].GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
	}

	private void CreateText() {
		if (dialogueManager.HasActions()) {
			int i = 1;
			foreach (var action in dialogueManager.GetActions()) {
				GameObject responseContainer = CreateTextContainer(responsePrefab, i);
				Text textComponent = responseContainer.GetComponentInChildren<Text>();
				textComponent.text = action.text;
				if (action.requirement > target.GetAffection()) {
					textComponent.color = Color.gray;
				}
				i++;
			}
		}

		StartCoroutine(
			ScrollText(
				dialogueManager.GetText(),
				CreateTextContainer(dialoguePrefab, 0)));

		MoveCursor();
	}

	private GameObject CreateTextContainer(GameObject prefab, int index) {
		RectTransform rectTransform = prefab.GetComponent<RectTransform>();
		float xOffset = 0;
		float yOffset = 0;

		if (index > 0) {
			RectTransform dialogueTransform = dialoguePrefab.GetComponent<RectTransform>();
			xOffset = index == 2 || index == 4
				? dialogueTransform.rect.width - rectTransform.rect.width
				: 0;
			yOffset = index == 3 || index == 4
				? -rectTransform.rect.height
				: 0;
			yOffset -= dialogueTransform.rect.height;
		}

		Vector3 position = new Vector3(
			transform.position.x + xOffset,
			transform.position.y + yOffset,
			transform.position.z);
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
			fullDialogue = text;
			Text content = container.GetComponentInChildren<Text>();
			for (int l = 0; l < text.Length; l += 1) {
				content.text = content.text + text.Substring(l, 1);
				yield return new WaitForSeconds(0.04f);
			}
			fullDialogue = null;
		}
	}
}