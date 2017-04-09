using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager {
	private InventoryController inventory;

	private DialogueNode[] currentDialogue;
	private int dialogueIndex;
	private int responseIndex;
	private string currentDialogueString;
	private Text activeContainer;

	public DialogueManager(InventoryController inventory) { this.inventory = inventory; }

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
			if (Input.GetAxis("Vertical") > 0) {
				responseIndex--;
			} else if (Input.GetAxis("Vertical") < 0) {
				responseIndex++;
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

	public List<Item> GetReward(NPCController target) {
		if (currentDialogue[dialogueIndex].actions.Length == 0) return null;

		Reward reward = currentDialogue[dialogueIndex]
			.actions[responseIndex]
			.reward;

		if (reward != null && reward.affection > 0) {
			target.UpdateAffection(reward.affection);
		}
		if (reward != null && reward.item.Count > 0) {
			inventory.inventory.items.AddRange(reward.item);
			return reward.item;
		}

		return null;
	}

	public bool isAllowed(int affection) {
		if (GetNextNode() == -1) return false;
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