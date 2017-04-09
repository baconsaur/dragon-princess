using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIDialogueRenderer : MonoBehaviour {
	public GUIPrefabs prefabs;
	private DialogueManager dialogueManager;

	void Start() {
		dialogueManager = new DialogueManager(
			GameObject.Find("Player").GetComponent<InventoryController>());
	}

	public List<GameObject> SetDialogue(NPCController target) {
		dialogueManager.SetCurrentDialogue(target.GetDialogue());
		return CreateText(target);
	}

	public void MoveCursor(List<GameObject> dialogueObjects) {
		if (dialogueObjects.Count > 1) {
			dialogueObjects[dialogueManager.GetCursor()]
				.GetComponent<Image>()
				.color = prefabs.defaultColor;
			dialogueObjects[dialogueManager.MoveCursor()]
				.GetComponent<Image>()
				.color = prefabs.selectionColor;
		}
	}

	public List<GameObject> SelectNode(NPCController target) {
		if (dialogueManager.StopScroll()) {
			StopAllCoroutines();
			return null;
		}

		if (dialogueManager.GetNextNode() < 0 || !dialogueManager.HasActions()) {
			return new List<GameObject>();
		}

		if (!dialogueManager.isAllowed(target.GetAffection())) return null;

		List<Item> rewards = dialogueManager.GetReward(target);
		if (rewards != null && rewards.Count > 0) {
			StartCoroutine(RenderRewards(rewards));
		}

		dialogueManager.SelectNode();
		return CreateText(target);
	}

	private List<GameObject> CreateText(NPCController target) {
		List<GameObject> dialogueObjects = new List<GameObject>();
		if (dialogueManager.HasActions()) {
			int i = 1;
			foreach (var action in dialogueManager.GetActions()) {
				GameObject responseContainer = CreateTextContainer(prefabs.responsePrefab, i);
				dialogueObjects.Add(responseContainer);

				Text textComponent = responseContainer.GetComponentInChildren<Text>();
				textComponent.text = action.text;
				if (action.requirement.affection > target.GetAffection()) {
					textComponent.color = Color.gray;
				}
				i++;
			}
		}

		GameObject textContainer = CreateTextContainer(prefabs.dialoguePrefab, 0);
		dialogueObjects.Add(textContainer);

		StartCoroutine(dialogueManager.ScrollText(textContainer));

		MoveCursor(dialogueObjects);
		return dialogueObjects;
	}

	private GameObject CreateTextContainer(GameObject prefab, int index) {
		RectTransform rectTransform = prefab.GetComponent<RectTransform>();
		float yOffset = 0;

		if (index > 1) {
			yOffset = -rectTransform.rect.height * (index - 1);
		}

		Vector3 position = new Vector3(
			transform.position.x,
			transform.position.y + yOffset,
			transform.position.z);
		GameObject textContainer = Instantiate(prefab, position, Quaternion.identity);

		textContainer.transform.Translate(prefab.transform.position);
		textContainer.transform.SetParent(transform);

		return textContainer;
	}

	private IEnumerator RenderRewards(List<Item> rewards) {
		GameObject rewardNotification = Instantiate(
			prefabs.notificationPrefab, transform.position, Quaternion.identity);
		Text content = rewardNotification.GetComponentInChildren<Text>();

		rewardNotification.transform.Translate(
			prefabs.notificationPrefab.transform.position);
		rewardNotification.transform.SetParent(transform);

		foreach (Item reward in rewards) {
			content.text = reward.name;
			yield return new WaitForSeconds(1.5f);
		}
		Destroy(rewardNotification);
	}
}