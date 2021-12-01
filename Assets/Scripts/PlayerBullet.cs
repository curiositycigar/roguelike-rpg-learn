using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 7.5f;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    public int damage = 50;

    // Start is called before the first frame update
    void Start()
    {
        // 子弹生命周期
        //Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.right跟随gameObject的旋转而旋转
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);

        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(damage);
        } else if (other.tag == "Boss")
        {
            BossController.instance.TakeDamage(damage);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
