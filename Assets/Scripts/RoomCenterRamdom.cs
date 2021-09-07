using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenterRamdom : RoomCenter
{
    public Transform enemiesParent;
    public Transform objectsParent;

    public List<GameObject> enemiePrefabs = new List<GameObject>();
    public List<GameObject> objectPrefabs = new List<GameObject>();

    // 生成几率
    public float generateProbability;

    // 生成范围和间隔
    private float horizontal = 7.5f;
    private float vertical = 3.5f;
    private float step = 1f;

    // Start is called before the first frame update
    protected override void Start()
    {
        GenerateItems();
        base.Start();
    }


    void GenerateItems()
    {
        for (float i = -horizontal; i < horizontal; i += step)
        {
            for (float j = -vertical; j < vertical; j += step)
            {
                if (Random.Range(0, 100) < generateProbability)
                {
                    CreateItem(transform.position + new Vector3(i, j, 0));
                }
            }
        }
    }

    void CreateItem(Vector3 position)
    {
        int random = Random.Range(0, 10);
        if (random < 8)
        {
            GameObject selcetObject = objectPrefabs[Random.Range(0, objectPrefabs.Count)];
            Instantiate(selcetObject, position, transform.rotation).transform.parent = objectsParent;
        } else
        {
            GameObject selcetEnemies = enemiePrefabs[Random.Range(0, enemiePrefabs.Count)];
            GameObject enemy = Instantiate(selcetEnemies, position, transform.rotation);
            enemy.transform.parent = enemiesParent;
            enemies.Add(enemy);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }
}
