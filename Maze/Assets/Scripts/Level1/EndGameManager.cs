using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour {

    private static bool game;

    public GameObject menu;
    public GameObject hero;


	void Start () {
        game = true;
        hero = GameObject.FindWithTag("Hero");
	}
	
	void Update () {
        

        if (!game)
        {

            if(hero == null)
                hero = GameObject.FindWithTag("Hero");

            menu.SetActive(true);
            hero.SetActive(false);
        }
    }

    public static void EndGame()
    {
        game = false;
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitButton()
    {
        Application.Quit();
    }


}
