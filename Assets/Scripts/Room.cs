using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("@Door Options")]
    public bool closeWhenEntered;


    [Header("@Door Prefabs")]
    public GameObject horizontalDoor;
    public GameObject verticalDoor;

    [Header("@Doors")]
    public bool upDoor;
    public bool rightDoor;
    public bool downDoor;
    public bool leftDoor;

    private List<GameObject> doors = new List<GameObject>();

    private float offsetY = 4.5f;
    private float offsetX = 8.5f;

    //public List<GameObject> enemies = new List<GameObject>();

    private GameObject outline;

    [HideInInspector]
    public bool roomActive;
    [HideInInspector]
    public GameObject roomLayout;

    private void Awake()
    {
        if (upDoor)
        {
            doors.Add(InstantiateInRoom(horizontalDoor, transform.position + new Vector3(0, offsetY, 0), transform.rotation));
        }
        if (downDoor)
        {
            doors.Add(InstantiateInRoom(horizontalDoor, transform.position + new Vector3(0, -offsetY, 0), transform.rotation));
        }
        if (leftDoor)
        {
            doors.Add(InstantiateInRoom(verticalDoor, transform.position + new Vector3(-offsetX, 0, 0), transform.rotation));
        }
        if (rightDoor)
        {
            doors.Add(InstantiateInRoom(verticalDoor, transform.position + new Vector3(offsetX, 0, 0), transform.rotation));
        }
        ToggleDoors(false);
        outline = GameObject.FindGameObjectWithTag("RoomOutline");
        outline.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    GameObject InstantiateInRoom(GameObject original, Vector3 positin, Quaternion rotation)
    {
        GameObject instance = Instantiate(original, positin, rotation);
        instance.transform.parent = gameObject.transform;
        return instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CameraController.instance.changeTarget(transform);

            if (closeWhenEntered)
            {
                ToggleDoors(true);
                closeWhenEntered = false;
            }

            ToggleOutline(true);
            roomLayout?.SetActive(true);

            roomActive = true;
        }
    }

    public void ToggleDoors(bool active)
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(active);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            roomActive = false;
        }
    }

    public void ToggleOutline(bool show)
    {
        outline?.SetActive(show);
    }
}
