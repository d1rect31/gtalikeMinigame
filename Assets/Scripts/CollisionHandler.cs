using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
            Time.timeScale = 0;
        }
        if (other.CompareTag("Coin"))
        {
            scoreManager.coins++;
            Destroy(other.gameObject);
        }
    }
}
