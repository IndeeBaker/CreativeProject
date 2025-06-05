using System.Collections;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform teleportTarget;
    public KeyCode interactKey = KeyCode.Q;

    private bool playerInRange = false;
    private GameObject player;
    private PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
            playerMovement = player.GetComponent<PlayerMovement>();
            // Show prompt UI if needed
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            playerMovement = null;
            // Hide prompt UI if needed
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            TeleportPlayerAndCamera();
        }
    }

    private void TeleportPlayerAndCamera()
    {
        // Move player instantly
        player.transform.position = teleportTarget.position;

        // Instantly move the camera to the new position (exit door)
        Vector3 camPos = Camera.main.transform.position;
        Vector3 targetCamPos = new Vector3(teleportTarget.position.x, teleportTarget.position.y, camPos.z);
        Camera.main.transform.position = targetCamPos;
    }
}



