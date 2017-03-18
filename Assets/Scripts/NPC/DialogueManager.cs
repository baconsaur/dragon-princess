using UnityEngine;

public class DialogueManager {
	private DialogueNode[] currentDialogue;
	private int dialogueIndex;
	private int responseIndex;

	public DialogueManager() { }

	public void SetCurrentDialogue(DialogueNode[] dialogue) {
		currentDialogue = dialogue;
		dialogueIndex = 0;
		responseIndex = 0;
	}

	public string GetText() { return currentDialogue[dialogueIndex].text; }

	public Action[] GetActions() { return currentDialogue[dialogueIndex].actions; }

	public bool HasActions() { return currentDialogue[dialogueIndex].actions.Length > 0; }

	public int MoveCursor() {
		if (currentDialogue[dialogueIndex].actions.Length > 0) {
			if (Input.GetAxis("Horizontal") > 0) {
				responseIndex += 1;
			} else if (Input.GetAxis("Horizontal") < 0) {
				responseIndex -= 1;
			} else if (Input.GetAxis("Vertical") > 0) {
				responseIndex -= 2;
			} else if (Input.GetAxis("Vertical") < 0) {
				responseIndex += 2;
			}

			int max = currentDialogue[dialogueIndex].actions.Length - 1 > 0
				? currentDialogue[dialogueIndex].actions.Length - 1
				: 0;

			responseIndex = Mathf.Clamp(responseIndex, 0, max);
		}

		return responseIndex;
	}

	public int GetNextNode() {
		if (currentDialogue[dialogueIndex].actions.Length > 0) {
			return currentDialogue[dialogueIndex].actions[responseIndex].nextNode;
		} else if (dialogueIndex < currentDialogue.Length -1) {
			return dialogueIndex + 1;
		}
		return -1;
	}

	public int SetNextNode() {
		dialogueIndex = GetNextNode();
		responseIndex = 0;
		return dialogueIndex;
	}

	public int GetCursor() { return responseIndex; }
}