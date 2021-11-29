using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Camera mainCamera, bigMapCamera;

    private bool bigMapActive = false;

    public float moveSpeed;

    public Transform target;

    public bool isBossRoom;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isBossRoom)
        {
            target = PlayerController.instance.transform;
        }

        ToggleBigMapActivate(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }

        if (!isBossRoom)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                ToggleBigMapActivate(true);
            }
            if (Input.GetKeyUp(KeyCode.M))
            {
                ToggleBigMapActivate(false);
            }
        }
    }

    public void changeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ToggleBigMapActivate(bool active)
    {
        if (!LevelManager.instance.isPaused)
        {
            bigMapActive = active;
            mainCamera.enabled = !bigMapActive;
            bigMapCamera.enabled = bigMapActive;

            PlayerController.instance.canMove = !active;

            Time.timeScale = active ? 0 : 1;

            UIController.instance.mapDisplay.SetActive(!active);
        }
    }
}
