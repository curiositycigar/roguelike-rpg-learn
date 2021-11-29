using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;

    public GameObject deletePannel;

    public PlayerSelector[] playerSelectors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void DeleteSave()
    {
        deletePannel.SetActive(true);
    }

    public void ConfirmDelete()
    {
        deletePannel.SetActive(false);
        foreach(PlayerSelector playerSelector in playerSelectors)
        {
            PlayerPrefs.SetInt(playerSelector.playerToSpawn.name, 0);
        }
    }

    public void CancelDelete()
    {
        deletePannel.SetActive(false);
    }
}
