using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {
	public Inventory inventory;

	void Start() { inventory.items = new List<Item>(); }

	public void AddItem(Item item) {
		if (item != null) {
			inventory.items.Add(item);
		}
	}

	public Item GetItem(int itemId) {
		return inventory.items.Find(item => item.id == itemId);
	}

	public bool HasItem(int itemId) {
		return inventory.items.Find(item => item.id == itemId) != null;
	}
}