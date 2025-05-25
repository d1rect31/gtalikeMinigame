using UnityEngine;

public class FlyerPoint : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlaceFlyer();
        }
    }

    void PlaceFlyer()
    {
        // You can add particle effects, sounds, etc. here
        Destroy(gameObject); // Make the flyer point disappear
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
