using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	public GameObject GUITextPrefab;

	private GameObject visibleText;
	private bool active;
	private DialogueNode[] currentDialogue;
	private int dialogueIndex;

	void Start() { active = false; }

	void Update() {
		if (active && Input.GetButtonDown("Fire1")) {
			if (dialogueIndex < currentDialogue.Length) {
				ShowNextText();
				dialogueIndex++;
			} else {
				EndDialogue();
			}
		}
	}

	public void StartDialogue(DialogueNode[] dialogue) {
		active = true;
		currentDialogue = dialogue;
		dialogueIndex = 0;

		ShowNextText();
	}

	public void EndDialogue() {
		Destroy(visibleText);
		active = false;
	}

	private void ShowNextText() {
		Destroy(visibleText);

		GameObject textContainer = Instantiate(GUITextPrefab, transform.position, Quaternion.identity);
		textContainer.transform.Translate(GUITextPrefab.transform.position);
		textContainer.transform.SetParent(transform);

		textContainer.GetComponentInChildren<Text>().text = currentDialogue[dialogueIndex].text;

		visibleText = textContainer;
	}

	public bool isActive() { return active; }
}