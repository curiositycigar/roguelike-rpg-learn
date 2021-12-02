using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int maxHealth;
    public int currentHealth;

    public float damageInvincLength = 1f;
    private float invincCount;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;

        //currentHealth = Mathf.Min(currentHealth, maxHealth);

        UIController.instance.SetHealth(maxHealth, currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (invincCount > 0)
        {
            invincCount -= Time.deltaTime;

            if (invincCount <= 0)
            {
                setPlayerAlpha(1);
            }
        }
    }

    public void DamagePlayer()
    {
        if (invincCount <= 0)
        {
            invincCount = damageInvincLength;
            setPlayerAlpha(.5f);

            currentHealth--;
            AudioManager.instance.PlaySFX(11);

            if (currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false);
                UIController.instance.deathScreen.SetActive(true);

                AudioManager.instance.PlaySFX(10);
                AudioManager.instance.PlayGameOver();
            }

            UIController.instance.SetHealth(maxHealth, currentHealth);
        }
    }

    void setPlayerAlpha(float r)
    {
        Color defaultColor = PlayerController.instance.bodySR.color;
        PlayerController.instance.bodySR.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, r);
    }

    public void setPlayerInvincible(float t)
    {
        invincCount = t;
        setPlayerAlpha(.5f);
    }

    public void UpgradeMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
        UIController.instance.SetHealth(maxHealth, currentHealth);
    }

    public bool HealPlayer(int healAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
            UIController.instance.SetHealth(maxHealth, currentHealth);
            return true;
        }
        return false;
    }
}
