using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject pauseScreen;
    public GameObject mainCamera;

    private GameObject[] players;

    void Awake()
    {
        mainCamera.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        //players = GameObject.FindGameObjectsWithTag("Player");
        //foreach (var player in players)
        //{
        //    player.gameObject.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // Pause game
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    public void StartGame()
    {
        foreach (var player in players)
        {
            player.gameObject.SetActive(true);
        }
        titleScreen.SetActive(false);
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true);
    }

    public void RestartGame()
    {
        foreach (var player in players)
        {
            player.gameObject.transform.localPosition = new Vector3(0, -6.41f, -266.4171f);
            player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            player.gameObject.transform.rotation = Quaternion.identity;
            player.gameObject.SetActive(false);
        }
        pauseScreen.SetActive(false);
        titleScreen.SetActive(true);
    }

    public void ExitGame()
    {
        #if UNITY_STANDALONE
                Application.Quit();
        #endif
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
