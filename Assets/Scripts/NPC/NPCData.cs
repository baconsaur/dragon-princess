using UnityEngine;

public class NPCData {
	private int affection;

	public NPCData() { affection = 1; }

	public int GetAffection() { return affection; }

	public void UpdateAffection(int change) {
		affection += change;
		if (affection < 0) affection = 0;
	}
}