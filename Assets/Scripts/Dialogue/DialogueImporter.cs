using UnityEngine;

public static class DialogueImporter {
	public static DialogueNode[] Import(TextAsset json) {
		DialogueCollection collection = JsonUtility.FromJson<DialogueCollection>(json.text);

		return collection.dialogueNodes;
	}
}