using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public float waitForAnyKey = 2f;

    public GameObject anyKeyText;

    public string mainMenuScreen;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(PlayerController.instance.gameObject);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForAnyKey > 0)
        {
            waitForAnyKey -= Time.deltaTime;
            if (waitForAnyKey <= 0)
            {
                anyKeyText.SetActive(true);
            }
        }  else
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScreen);
            }
        }
    }
}
