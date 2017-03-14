using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour {
	public TextAsset json;
	public DialogueNode[] dialogueNodes;

    public void Start() { dialogueNodes = DialogueImporter.Import(json); }

    public void Update() { }

	public DialogueNode[] getDialogue() { return dialogueNodes; }
}