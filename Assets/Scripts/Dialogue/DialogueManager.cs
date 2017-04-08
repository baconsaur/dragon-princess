using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager {
	private DialogueNode[] currentDialogue;
	private int dialogueIndex;
	private int responseIndex;
	private string currentDialogueString;
	private Text activeContainer;

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

	public void SelectNode() {
		dialogueIndex = GetNextNode();
		responseIndex = 0;
	}

	public Reward GetReward() {
		if (currentDialogue[dialogueIndex].actions.Length == 0) return null;

		return currentDialogue[dialogueIndex]
			.actions[responseIndex].reward;
	}

	public bool isAllowed(int affection) {
		if (currentDialogue[dialogueIndex].actions.Length == 0) return true;

		return affection >= currentDialogue[dialogueIndex]
			    .actions[responseIndex]
			    .requirement.affection;
	}

	public int GetCursor() { return responseIndex; }

	public bool StopScroll() {
		if (currentDialogueString != null) {
			activeContainer.text = currentDialogueString;
			currentDialogueString = null;
			return true;
		}
		return false;
	}

	public IEnumerator ScrollText(GameObject container) {
		if (container != null) {
			currentDialogueString = GetText();
			Text content = container.GetComponentInChildren<Text>();
			activeContainer = content;

			for (int l = 0; l < currentDialogueString.Length; l += 1) {
				content.text = content.text + currentDialogueString.Substring(l, 1);
				yield return new WaitForSeconds(0.04f);
			}
			currentDialogueString = null;
		}
	}
}