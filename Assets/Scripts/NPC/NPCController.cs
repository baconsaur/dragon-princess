using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour {
	public DialogueSet[] dialogueSet;
	public DialogueNode[] dialogueNodes;
	public NPCData npcData;
	private int dialogueIndex;

	public void Start() {
		if (dialogueSet.Length > 0) {
			SetDialogue(0);
		}
	}

	public void Update() { }

	public int GetAffection() { return npcData.affection; }

	public void UpdateAffection(int change) {
		npcData.affection += change;

		for (int i = dialogueSet.Length - 1; i > dialogueIndex; i--) {
			if (npcData.affection >= dialogueSet[i].unlockLevel) {
				SetDialogue(i);
				break;
			}
		}
	}

	public DialogueNode[] GetDialogue() { return dialogueNodes; }

	private void SetDialogue(int index) {
		dialogueIndex = index;
		dialogueNodes = DialogueImporter.Import(dialogueSet[dialogueIndex].json);
	}
}