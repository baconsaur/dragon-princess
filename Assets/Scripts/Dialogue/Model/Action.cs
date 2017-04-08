using System;

[Serializable]
public class Action {
	public string text;
	public Requirement requirement;
	public Reward reward;
	public int nextNode;
}