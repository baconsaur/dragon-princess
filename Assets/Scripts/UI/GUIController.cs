using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
	public GameObject dialoguePrefab;
	public GameObject responsePrefab;
	public GameObject notificationPrefab;

	private InventoryController inventory;

	private DialogueManager dialogueManager;
	private List<GameObject> dialogueObjects;
	private GameObject activeNotification;
	private bool active;
	private float cooldown;
	private NPCController target;

	void Start() {
		inventory = GameObject.Find("Player").GetComponent<InventoryController>();

		active = false;
		dialogueObjects = new List<GameObject>();
		dialogueManager = new DialogueManager();
	}

	void Update() {
		cooldown -= Time.deltaTime;
		if (cooldown > 0) return;
		if (!active) {
			if (activeNotification != null) {
				PerformAction();
			}
			return;
		}

		if (Input.GetButton("Fire1") || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) {
			MoveCursor();
			PerformAction();
			cooldown = 0.3f;
		}
	}

	public bool isActive() { return active; }

	public void StartDialogue(NPCController npc) {
		this.target = npc;
		dialogueManager.SetCurrentDialogue(npc.GetDialogue());

		cooldown = 0.3f;
		active = true;

		CreateText();
	}

	private void PerformAction() {
		if (Input.GetButtonDown("Fire1")) {
			if (activeNotification != null) {
				Destroy(activeNotification);
				return;
			}

			StopAllCoroutines();
			if (dialogueManager.StopScroll()) return;

			SelectNode();
		}
	}

	private void SelectNode() {
		if (!dialogueManager.isAllowed(target.GetAffection())) return;

		DestroyText();

		Reward reward = dialogueManager.GetReward();
		if (reward != null) {
			target.UpdateAffection(reward.affection);

			if (reward.item.Count > 0) {
				Item rewardItem = reward.item[0];
				inventory.AddItem(rewardItem);

				GameObject notificationContainer = CreateTextContainer(notificationPrefab, 0);
				Text textComponent = notificationContainer.GetComponentInChildren<Text>();
				textComponent.text = textComponent.text.Replace("[item]", rewardItem.name);

				activeNotification = notificationContainer;
			}
		}

		if (dialogueManager.GetNextNode() < 0 || !dialogueManager.HasActions()) {
			active = false;
			target = null;
			return;
		}

		dialogueManager.SelectNode();
		CreateText();
	}

	private void MoveCursor() {
		dialogueObjects[dialogueManager.GetCursor()].GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;
		dialogueObjects[dialogueManager.MoveCursor()].GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
	}

	private void CreateText() {
		if (dialogueManager.HasActions()) {
			int i = 1;
			foreach (var action in dialogueManager.GetActions()) {
				GameObject responseContainer = CreateTextContainer(responsePrefab, i);
				dialogueObjects.Add(responseContainer);

				Text textComponent = responseContainer.GetComponentInChildren<Text>();
				textComponent.text = action.text;
				if (action.requirement.affection > target.GetAffection()) {
					textComponent.color = Color.gray;
				}
				i++;
			}
		}

		GameObject textContainer = CreateTextContainer(dialoguePrefab, 0);
		dialogueObjects.Add(textContainer);

		StartCoroutine(
			dialogueManager.ScrollText(
				textContainer));

		MoveCursor();
	}

	private GameObject CreateTextContainer(GameObject prefab, int index) {
		RectTransform rectTransform = prefab.GetComponent<RectTransform>();
		float xOffset = 0;
		float yOffset = 0;

		if (index > 0) {
			RectTransform dialogueTransform = dialoguePrefab.GetComponent<RectTransform>();
			xOffset = index == 2 || index == 4
				? dialogueTransform.rect.width - rectTransform.rect.width
				: 0;
			yOffset = index == 3 || index == 4
				? -rectTransform.rect.height
				: 0;
			yOffset -= dialogueTransform.rect.height;
		}

		Vector3 position = new Vector3(
			transform.position.x + xOffset,
			transform.position.y + yOffset,
			transform.position.z);
		GameObject textContainer = Instantiate(prefab, position, Quaternion.identity);

		textContainer.transform.Translate(prefab.transform.position);
		textContainer.transform.SetParent(transform);

		return textContainer;
	}

	private void DestroyText() {
		foreach (GameObject textObject in dialogueObjects) {
			Destroy(textObject);
		}
		dialogueObjects = new List<GameObject>();
	}
}