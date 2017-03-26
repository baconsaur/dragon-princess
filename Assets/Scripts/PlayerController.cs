using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public float moveSpeed;
    public float rotationSpeed;

	private Animator animator;

    private GUIController GuiController;

    void Start() {
        GuiController = GameObject.Find("Canvas").GetComponent<GUIController>();
	    animator = GetComponent<Animator>();
        enabled = true;
    }

    void Update() {
	    if (GuiController.isActive()) return;

	    Vector3 xDirection = Camera.main.transform.parent.TransformDirection(Vector3.forward);
	    xDirection.y = 0;

        float z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime;

	    if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0) {
		    animator.SetBool("walking", false);
		    return;
	    }

	    transform.rotation = Quaternion.LookRotation(xDirection);
	    transform.Translate(x, 0, z);

    	if (Input.GetAxis("Vertical") > -0.7f && Input.GetAxis("Vertical") < 0.7f) {
		    animator.SetBool("walking", true);
	    }

	    if (Input.GetAxis("Vertical") > 0.7f || Input.GetAxis("Vertical") < -0.7f) {
			animator.SetBool("running", true);
		} else {
			animator.SetBool("running", false);
		}
    }

	public void OnTriggerStay(Collider other) {
		if (!GuiController.isActive() && other.CompareTag("NPC") && Input.GetButtonDown("Fire1")) {
			NPCController npcController = other.GetComponent<NPCController>();
			GuiController.StartDialogue(npcController);
		}
	}
}