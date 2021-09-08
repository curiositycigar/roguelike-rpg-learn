using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public Rigidbody2D rb;
    public Transform gunArm;
    public Animator anim;

    //public GameObject bulletToFire;
    //public Transform firePoint;
    //public float shotCycle;
    //private float shotCounter;

    private Camera mainCam;
    private Vector2 moveInput;


    public SpriteRenderer bodySR;

    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f, dashInvincibility = .5f;
    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;

    [HideInInspector]
    public bool canMove = true;

    public List<Gun> availableGuns = new List<Gun>();
    private int currentGun;

    private void Awake()
    {
        instance = this;
        activeMoveSpeed = moveSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;

        UIController.instance.SetCurrentGun(availableGuns[currentGun].GunUI, availableGuns[currentGun].weaponName);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();

            //transform.position += new Vector3(moveInput.x * Time.deltaTime * moveSpeed, moveInput.y * Time.deltaTime * moveSpeed, 0);

            rb.velocity = moveInput * activeMoveSpeed;

            Vector3 mousePosition = Input.mousePosition;
            Vector3 screenPosition = mainCam.WorldToScreenPoint(transform.position);

            if (mousePosition.x > screenPosition.x)
            {
                transform.localScale = Vector3.one;
                gunArm.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                gunArm.localScale = new Vector3(-1, -1, 1);
            }


            // rotate gun arm
            Vector2 offset = new Vector2(mousePosition.x - screenPosition.x, mousePosition.y - screenPosition.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);

            //if (Input.GetMouseButtonDown(0))
            //{
            //    Shoot();
            //}

            //if (Input.GetMouseButton(0))
            //{
            //    shotCounter -= Time.deltaTime;
            //    if (shotCounter <= 0)
            //    {
            //        Shoot();
            //    }
            //}

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (availableGuns.Count > 0)
                {
                    currentGun = (currentGun + 1) % availableGuns.Count;
                    SwithGun();
                } else
                {
                    Debug.LogError("Player has no guns!");
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    PlayerHealthController.instance.setPlayerInvincible(dashInvincibility);

                    anim.SetTrigger("dash");

                    AudioManager.instance.PlaySFX(8);
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }



            if (moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        } else
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
    }

    //private void Shoot()
    //{
    //    AudioManager.instance.PlaySFX(12);
    //    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
    //    shotCounter = shotCycle;
    //}

    public void SwithGun()
    {
        foreach (Gun gun in availableGuns)
        {
            gun.gameObject.SetActive(false);
        }
        availableGuns[currentGun].gameObject.SetActive(true);
        UIController.instance.SetCurrentGun(availableGuns[currentGun].GunUI, availableGuns[currentGun].weaponName);
    }

    public void PickupGun(Gun pickupGun)
    {
        bool hasGun = false;
        foreach(Gun gun in availableGuns)
        {
            if (gun.weaponName == pickupGun.weaponName)
            {
                hasGun = true;
            }
        }
        if (!hasGun)
        {
            Gun gunClone = Instantiate(pickupGun);
            gunClone.transform.parent = gunArm;
            gunClone.transform.localPosition = Vector3.zero;
            gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
            gunClone.transform.localScale = Vector3.one;
            availableGuns.Add(gunClone);
        }
        currentGun = availableGuns.Count - 1;
        SwithGun();
    }
}
