using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("@Basic Options")]
    public Rigidbody2D enemyRigidbody;
    public SpriteRenderer body;
    public Animator anim;
    public int health = 150;
    public float speed;
    private Vector3 moveDirection;

    [Header("@Game Objects")]
    public GameObject[] deathSplatters;
    public GameObject hitEffect;
    public GameObject bullet;

    [Header("@Shoot Options")]
    public bool shouldShoot;
    public Transform firePoint;
    public float fireRate;
    public float shootRange;
    private float fireCounter;

    [Header("@Enemy Chase")]
    public bool shouldChasePlayer;
    public float rangeToChasePlayer;

    [Header("@Enemy Runaway")]
    public bool shouleRunaway;
    public float runwayRange;

    [Header("@Enemy Wander")]
    public bool shouleWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    [Header("@Enemy Patrol")]
    public bool shoulePatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

    [Header("@Enemy Drop")]
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPerent;

    // Start is called before the first frame update
    void Start()
    {
        if (shouleWander)
        {
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        }
        // TODO: 制作更多的RoomCenter
    }

    // Update is called once per frame
    void Update()
    {
        Transform playerTransform = PlayerController.instance.transform;
        if (body.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;
            if (shouldChasePlayer && Vector3.Distance(transform.position, playerTransform.position) <= rangeToChasePlayer)
            {
                moveDirection = playerTransform.position - transform.position;
            } else if (shouleWander)
            {
                if (wanderCounter > 0)
                {
                    wanderCounter -= Time.deltaTime;

                    moveDirection = wanderDirection;

                    if (wanderCounter <= 0)
                    {
                        pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                    }
                }
                if (pauseCounter > 0)
                {
                    pauseCounter -= Time.deltaTime;

                    if (pauseCounter <= 0)
                    {
                        wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                        wanderDirection = new Vector3(Random.Range(-1f,  1f), Random.Range(-1f, 1f), 0);
                    }
                }
            } else if (shoulePatrol)
            {
                moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

                if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < .2f)
                {
                    currentPatrolPoint++;

                    if (currentPatrolPoint >= patrolPoints.Length)
                    {
                        currentPatrolPoint = 0;
                    }
                }
            }

            if (shouleRunaway && Vector3.Distance(transform.position, playerTransform.position) <= runwayRange)
            {
                moveDirection = transform.position - playerTransform.position;
            }

            moveDirection.Normalize();

            enemyRigidbody.velocity = moveDirection * speed;

            if (shouldShoot && Vector3.Distance(playerTransform.position, transform.position) <= shootRange)
            {
                fireCounter -= Time.deltaTime;

                if (fireCounter <= 0)
                {
                    Shoot();
                }
            }
        }  else
        {
            enemyRigidbody.velocity = Vector3.zero;
        }


        if (moveDirection == Vector3.zero)
        {
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);
        }
    }

    void DropItem()
    {
        if (shouldDropItem)
        {
            float dropChange = Random.Range(0, 100f);

            if (dropChange < itemDropPerent)
            {
                GameObject randItem = itemsToDrop[Random.Range(0, itemsToDrop.Length)];
                Instantiate(randItem, transform.position, transform.rotation);
            }
        }
    }

    private void Shoot()
    {
        fireCounter = fireRate;
        AudioManager.instance.PlaySFX(13);
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;

        AudioManager.instance.PlaySFX(2);

        Instantiate(hitEffect, transform.position, transform.rotation);

        if (health <= 0)
        {
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(1);

            GameObject deathSplatter = deathSplatters[Random.Range(0, deathSplatters.Length)];

            int rotation = Random.Range(0, 4);

            Instantiate(deathSplatter, transform.position, Quaternion.Euler(0, 0, rotation * 90));

            DropItem();
        }
    }
}
