using System;

[Serializable]
public class DialogueNode {
	public int id;
	public string text;
	public Action[] actions;
}