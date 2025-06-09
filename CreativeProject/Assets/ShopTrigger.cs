using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public GameObject shopUI; // Assign your shop panel here in Inspector

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            shopUI.SetActive(!shopUI.activeSelf);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            shopUI.SetActive(false); // Auto close when leaving
        }
    }
}
