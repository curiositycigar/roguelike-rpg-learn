using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;

    public float pickupCountdown = .5f;

    private void Awake() {
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (pickupCountdown > 0)
        {
            pickupCountdown -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && pickupCountdown <= 0)
        {
            LevelManager.instance.GetCoins(coinValue);
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(5);
        }
    }
}
