using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float moveSpeed;
    public float rotationSpeed;

    private GUIController GuiController;

    void Start() {
        GuiController = GameObject.Find("Canvas").GetComponent<GUIController>();
        enabled = true;
    }

    void Update() {
	    if (GuiController.isActive()) return;

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }

	public void OnTriggerStay(Collider other) {
		if (!GuiController.isActive() && other.tag == "NPC" && Input.GetButtonDown("Fire1")) {
			NPCController npcController = other.GetComponent<NPCController>();
			GuiController.StartDialogue(npcController.getDialogue());
		}
	}
}