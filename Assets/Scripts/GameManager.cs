using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Ключи")]
    public GameObject keyPrefab;
    public int maxKeys, curKeys;
    public List<GameObject> keys;
    [Header("Объекты карты")]
    public GameObject[] monsterGates;
    public GameObject exitGate;

    private void Start()
    {
        instance = this;
        MapConfig();
    }
    private void FixedUpdate()
    {
        OpenGates();
    }
    private void MapConfig()
    {
        if (maxKeys == 0)
            maxKeys = 1;
        for (int i = 0; i < maxKeys; i++)
        {
            Vector2 pos2 = KuplinovWalk.Map[Random.Range(1, KuplinovWalk.Map.Count)].pos; //point[0] == player position
            keys.Add(Instantiate(keyPrefab, new Vector3(pos2.x, .5f, pos2.y), Quaternion.identity));
        }
    }
    private void OpenGates()
    {
        if (curKeys == maxKeys)
        {
            curKeys = -1;
            foreach (GameObject g in monsterGates)
                g.SetActive(false);
        }
    }
}
