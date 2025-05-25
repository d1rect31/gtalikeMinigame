using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void RestartScene()
    {
        Time.timeScale = 1f; // Resume time in case it was paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
