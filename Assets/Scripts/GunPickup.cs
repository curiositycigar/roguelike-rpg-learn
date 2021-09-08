using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public Gun theGun;

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
            PlayerController.instance.PickupGun(theGun);
            AudioManager.instance.PlaySFX(7);
            Destroy(gameObject);
        }
    }
}
