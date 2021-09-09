using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public GameObject buyMessage;

    public bool isHealthRestore, isHealthUpgrade, isWeapon;

    public int healthUpgradeAmount = 1;

    public int itemCost;

    public Gun[] potentialGuns;
    public SpriteRenderer gunSprite;
    private Gun selectGun;

    public Text infoText;

    private bool inBuyZone;

    // Start is called before the first frame update
    void Start()
    {
        generateGun();
    }

    // Update is called once per frame
    void Update()
    {
        if (inBuyZone)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (LevelManager.instance.currentCoins >=  itemCost)
                {
                    LevelManager.instance.spendCoins(itemCost);

                    if (isHealthRestore)
                    {
                        PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);
                    }
                    if (isHealthUpgrade)
                    {
                        PlayerHealthController.instance.UpgradeMaxHealth(healthUpgradeAmount);
                    }
                    if (isWeapon)
                    {
                        PlayerController.instance.PickupGun(selectGun);
                    }

                    gameObject.SetActive(false);
                    inBuyZone = false;

                    AudioManager.instance.PlaySFX(18);
                } else
                {
                    AudioManager.instance.PlaySFX(19);
                }
            }
        }
    }

    private void generateGun()
    {
        if (isWeapon)
        {
            selectGun = potentialGuns[Random.Range(0, potentialGuns.Length)];
            gunSprite.sprite = selectGun.gunShopSprite;
            itemCost = selectGun.itemCost;
            infoText.text = "Buy " + selectGun.weaponName + "\n- " + selectGun.itemCost + " -";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyMessage.SetActive(true);

            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            buyMessage.SetActive(false);

            inBuyZone = false;
        }
    }
}
