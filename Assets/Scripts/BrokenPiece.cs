using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiece : MonoBehaviour
{
    public float moveSpeed = 3;
    private Vector3 moveDirection;

    public float deceleation = 5f;

    public float lifeTime = 3f;

    public SpriteRenderer picSR;
    public float fadeSpeed = 2.5f;



    // Start is called before the first frame update
    void Start()
    {
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Time.deltaTime * moveDirection;

        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleation * Time.deltaTime);

        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            picSR.color = new Color(picSR.color.r, picSR.color.g, picSR.color.b, Mathf.MoveTowards( picSR.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (picSR.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
