using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    [Header("@Debug Options")]
    public Color startColor;
    public Color endColor, shopColor;

    public enum Direction
    {
        up, right, down, left,
    }

    [Header("@Base Options")]
    public Direction selectDirection;
    public float xOffset = 18f;
    public float yOffset = 10f;

    public LayerMask whatIsRoom;

    public int distanceToEnd;
    public Transform generationPoint;

    [Header("@Shop")]
    public bool incldeShop;
    public int minDistanceToShop, maxDistanceToShop;

    private GameObject startRoom;
    private GameObject endRoom;
    private GameObject shopRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    private List<GameObject> generatedOutlines = new List<GameObject>();

    [Header("@Room Prefabs")]

    public GameObject layoutRoom;
    public RoomPrefabs rooms;
    public RoomCenter roomCenterStart, roomCenterEnd, roomCenerShop;
    public RoomCenter[] potentialCenters;

    // Start is called before the first frame update
    void Start()
    {
        InitRoomsLayout();

        InitRoomOutlines();

        InitRoomCenter();

        HideRoomLayouts();
    }

    // 初始化房间layout
    private void InitRoomsLayout()
    {
        startRoom = Instantiate(layoutRoom, generationPoint.position, generationPoint.rotation);
        startRoom.GetComponent<SpriteRenderer>().color = startColor;

        selectDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for (int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generationPoint.position, generationPoint.rotation);

            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                endRoom = newRoom;
            }
            else
            {
                layoutRoomObjects.Add(newRoom);
            }

            selectDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generationPoint.position, .2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }
        }

        if (incldeShop)
        {
            int shopSelect = Random.Range(minDistanceToShop, maxDistanceToShop + 1);

            shopRoom = layoutRoomObjects[shopSelect];

            layoutRoomObjects.RemoveAt(shopSelect);

            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }
    }

    // 初始化房间outline(wall)
    private void InitRoomOutlines()
    {
        // create room outlines
        CreateRoomOutline(startRoom);
        foreach (GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room);
        }
        CreateRoomOutline(endRoom);
        if (incldeShop)
        {
            CreateRoomOutline(shopRoom);
        }
    }

    // 初始化房间、初始化敌人
    private void InitRoomCenter()
    {
        foreach (GameObject outline in generatedOutlines)
        {
            if (outline.transform.position == startRoom.transform.position)
            {
                Instantiate(roomCenterStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(roomCenterEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else if (incldeShop && outline.transform.position == shopRoom.transform.position)
            {
                Instantiate(roomCenerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);
                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }

    void HideRoomLayouts()
    {
        startRoom.SetActive(false);
        endRoom.SetActive(false);
        shopRoom.SetActive(false);
        foreach (GameObject room in layoutRoomObjects)
        {
            room.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    void MoveGenerationPoint()
    {
        switch(selectDirection)
        {
            case Direction.up:
                generationPoint.position += new Vector3(0, yOffset, 0);
                break;
            case Direction.down:
                generationPoint.position += new Vector3(0, -yOffset, 0);
                break;
            case Direction.left:
                generationPoint.position += new Vector3(-xOffset, 0, 0);
                break;
            case Direction.right:
                generationPoint.position += new Vector3(xOffset, 0, 0);
                break;
        }
    }

    int getInt(bool val)
    {
        return val ? 1 : 0;
    }

    public void CreateRoomOutline(GameObject roomLayout)
    {
        Vector3 roomPosition = roomLayout.transform.position;
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), .2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), .2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), .2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), .2f, whatIsRoom);

        // TODO: 生成rooms
        string bString = $"{getInt(roomAbove)}{getInt(roomRight)}{getInt(roomBelow)}{getInt(roomLeft)}";

        GameObject room = rooms.map[bString];

        if (room)
        {
            GameObject roomOutline = Instantiate(room, roomPosition, transform.rotation);
            roomOutline.GetComponent<Room>().roomLayout = roomLayout;
            generatedOutlines.Add(roomOutline);
        }

    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleRight, singleDown, singleLeft,
        doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleDownLeft, doubleLeftUp,
        tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
        fourway;

    [HideInInspector]
    public Dictionary<string, GameObject> map {
        get {
            Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
            dictionary.Add("1000", singleUp);
            dictionary.Add("0100", singleRight);
            dictionary.Add("0010", singleDown);
            dictionary.Add("0001", singleLeft);
            dictionary.Add("1010", doubleUpDown);
            dictionary.Add("0101", doubleLeftRight);
            dictionary.Add("1100", doubleUpRight);
            dictionary.Add("0110", doubleRightDown);
            dictionary.Add("0011", doubleDownLeft);
            dictionary.Add("1001", doubleLeftUp);
            dictionary.Add("1110", tripleUpRightDown);
            dictionary.Add("0111", tripleRightDownLeft);
            dictionary.Add("1011", tripleDownLeftUp);
            dictionary.Add("1101", tripleLeftUpRight);
            dictionary.Add("1111", fourway);
            return dictionary;
        }
    }
}