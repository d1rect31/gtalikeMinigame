using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenes : MonoBehaviour
{
    public void ChangeScenes(int numberScenes)
    {
        SceneManager.LoadScene(numberScenes);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
