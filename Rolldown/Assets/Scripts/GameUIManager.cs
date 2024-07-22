using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public GameObject pauseScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }
    public void PauseGame()
    {
        if (pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(false);
        } else
        {
            pauseScreen.SetActive(true);
        }
        
    }

    public void ExitGame()
    {
        BootstrapManager.instance.ExitGame();
    }
}
