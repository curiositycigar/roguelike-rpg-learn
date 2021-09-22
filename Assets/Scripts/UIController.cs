using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    static public UIController instance;

    [Header("@ Health UI")]
    public Slider healthSlider;
    public Text healthText;

    [Header("@ Coin UI")]
    public Text coinText;

    [Header("@ Gun UI")]
    public Image currentGun;
    public Text currentGunName;

    [Header("@ Fade Options")]
    public Image fadeScreen;
    public float fadeSpeed;
    private bool fadeToBlack, fadeOutBlack;

    [Header("@ Scenes")]
    public string newGameScene;
    public string mainMenuScene;

    [Header("@ UI Screens")]
    public GameObject deathScreen;
    public GameObject pauseMenu;

    public GameObject mapDisplay;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;

        Gun currentGun = PlayerController.instance.currentGun;
        SetCurrentGun(currentGun.GunUI, currentGun.weaponName);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOutBlack)
        {
            Color c = fadeScreen.color;
            fadeScreen.color = new Color(c.r, c. g, c.b, Mathf.MoveTowards(c.a, 0, fadeSpeed * Time.deltaTime));
            if (c.a == 0f)
            {
                fadeOutBlack = false;
            }
        }
        if (fadeToBlack)
        {
            Color c = fadeScreen.color;
            fadeScreen.color = new Color(c.r, c.g, c.b, Mathf.MoveTowards(c.a, 1f, fadeSpeed * Time.deltaTime));
            if (c.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }

    // 外部设置
    public void SetHealth(int max, int current)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
        healthText.text = current + " / " + max;
    }

    // 外部设置
    public void SetCurrentGun(Sprite sprite, string name)
    {
        currentGun.sprite = sprite;
        currentGunName.text = name;
    }

    public void setCoins(int count)
    {
        coinText.text = count.ToString();
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void NewGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(newGameScene);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnPause();
    }
}
