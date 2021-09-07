using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public bool openWhenCleared;

    public List<GameObject> enemies = new List<GameObject>();

    [HideInInspector]
    public Room theRoom;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (openWhenCleared && enemies.Count > 0)
        {
            theRoom.closeWhenEntered = true;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (enemies.Count > 0 && theRoom.roomActive && openWhenCleared)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }

            if (enemies.Count == 0)
            {
                theRoom.ToggleDoors(false);
            }
        }

    }
}
