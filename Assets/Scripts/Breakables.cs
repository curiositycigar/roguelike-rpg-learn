using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] brokenPices;

    // 可以抽离出去，通过对BoxPiece应用脚本
    public int maxPieces = 5;


    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPerent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyBox()
    {
        Destroy(gameObject);

        AudioManager.instance.PlaySFX(0);

        int piecesToDrop = Random.Range(1, maxPieces);

        for (int i = 0; i < piecesToDrop; i++)
        {
            GameObject randomPice = brokenPices[Random.Range(0, brokenPices.Length)];

            Instantiate(randomPice, transform.position, transform.rotation);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && PlayerController.instance.dashCounter > 0 || other.tag == "PlayerBullet")
        {
            DestroyBox();
            DropItem();
        }
    }
}
