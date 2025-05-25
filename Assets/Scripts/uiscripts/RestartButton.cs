using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartButton : MonoBehaviour
{
    [Header("������ ������ ��� �����������")]
    public int levelIndex = 0;

    // ���� ����� ����� ��������� �� ������� OnClick ������ � ����������
    public void RestartLevel()
    {
        SceneManager.LoadScene(levelIndex);
    }
}
