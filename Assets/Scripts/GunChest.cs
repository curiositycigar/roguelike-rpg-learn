using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : MonoBehaviour
{
    public GunPickup[] potentialGuns;

    public SpriteRenderer theSR;
    public Sprite chestOpen;

    public GameObject notification;

    private bool canOpen;
    private bool opened = false;
    private float scaleSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.E))
        {
            GunPickup selectGun = potentialGuns[Random.Range(0, potentialGuns.Length)];
            theSR.sprite = chestOpen;
            Instantiate(selectGun, transform.position + new Vector3(0, -.5f, 0), transform.rotation);
            notification.SetActive(false);
            opened = true;
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }

        if (opened && transform.localScale != Vector3.one)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, scaleSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (opened == false && other.tag == "Player")
        {
            notification.SetActive(true);
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            notification.SetActive(false);
            canOpen = false;
        }
    }
}
