using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;

    private GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            player.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        foreach (var player in players)
        {
            player.gameObject.SetActive(true);
        }
        titleScreen.SetActive(false);
    }
}
