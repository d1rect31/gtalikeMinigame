using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(1);
                }
            }
        }
        if (other.CompareTag("Coin"))
        {
            scoreManager.coins++;
            Destroy(other.gameObject);
        }
    }
}
