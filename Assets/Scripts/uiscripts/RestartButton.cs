using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartButton : MonoBehaviour
{
    [Header("Индекс уровня для перезапуска")]
    public int levelIndex = 0;

    // Этот метод можно назначить на событие OnClick кнопки в инспекторе
    public void RestartLevel()
    {
        SceneManager.LoadScene(levelIndex);
    }
}
