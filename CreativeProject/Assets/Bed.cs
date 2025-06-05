using UnityEngine;

public class Bed : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.Q;
    private bool playerInRange = false;
    private TimeManager timeManager;
    private GameObject player;
    private PlayerMovement playerMovement;

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
            playerMovement = player.GetComponent<PlayerMovement>();
            // Show "Press Q to sleep" UI here if you want
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerMovement = null;
            player = null;
            // Hide sleep UI prompt here if you have one
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            Sleep();
        }
    }

    void Sleep()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false; // Disable player movement while sleeping
        }

        timeManager.Sleep();

        // Optionally, add a short delay here and re-enable movement:
        StartCoroutine(WakeUp());
    }

    System.Collections.IEnumerator WakeUp()
    {
        // Simulate sleep delay, e.g. 2 seconds
        yield return new WaitForSeconds(2f);

        if (playerMovement != null)
        {
            playerMovement.enabled = true; // Enable movement again after waking up
        }
        // Hide any sleep UI here if you added it
    }
}
