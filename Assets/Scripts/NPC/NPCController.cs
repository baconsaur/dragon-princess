using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour {
	public DialogueSet[] dialogueSet;
	public DialogueNode[] dialogueNodes;
	private NPCData npcDdata;
	private int dialogueIndex;

	public void Start() {
		if (dialogueSet.Length > 0) {
			SetDialogue(0);
		}
		npcDdata = new NPCData();
	}

	public void Update() { }

	public int GetAffection() { return npcDdata.GetAffection(); }

	public void UpdateAffection(int change) {
		npcDdata.UpdateAffection(change);

		for (int i = dialogueSet.Length - 1; i > dialogueIndex; i--) {
			if (npcDdata.GetAffection() >= dialogueSet[i].unlockLevel) {
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