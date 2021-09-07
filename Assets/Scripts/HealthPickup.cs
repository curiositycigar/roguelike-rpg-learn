using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount;

    public float pickupCountdown = .5f;

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
            bool used = PlayerHealthController.instance.HealPlayer(healAmount);
            if (used)
            {
                AudioManager.instance.PlaySFX(7);
                Destroy(gameObject);
            }
        }
    }
}
