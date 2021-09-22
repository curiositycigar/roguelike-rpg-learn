using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToLoad = 4f;

    public string nextLevel;

    public bool isPaused;

    public int currentCoins;

    public Transform startPoint;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.transform.position = startPoint.position;
        PlayerController.instance.canMove = false;

        currentCoins = CharacterTracker.instance.currentCoins;

        Time.timeScale = 1;
        UIController.instance.setCoins(currentCoins);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnPause();
        }
    }

    public IEnumerator LevelEnd()
    {
        AudioManager.instance.PlayVictory();

        PlayerController.instance.canMove = false;

        UIController.instance.StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        // 离开场景时记录玩家数据
        CharacterTracker.instance.currentCoins = currentCoins;
        CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;

        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnPause()
    {
        if (!isPaused)
        {
            UIController.instance.pauseMenu.SetActive(true);
            isPaused = true;

            Time.timeScale = 0;
        } else
        {
            UIController.instance.pauseMenu.SetActive(false);
            isPaused = false;

            Time.timeScale = 1;
        }
    }

    public void GetCoins(int amount)
    {
        currentCoins += amount;
        UIController.instance.setCoins(currentCoins);
    }

    public void spendCoins(int amount)
    {
        currentCoins -= amount;

        if (currentCoins < 0)
        {
            currentCoins = 0;
        }

        UIController.instance.setCoins(currentCoins);

    }
}
