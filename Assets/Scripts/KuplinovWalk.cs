using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuplinovWalk : MonoBehaviour
{
    public GameObject mapPoints;

    public Vector3 destination;

    public bool[] no_ways = new bool[3];
    bool isReady = true;

    public static List<point> Map;
    private List<point> Way;

    float toRotate;
    Vector3 posToGo;

    private void Awake()
    {
        posToGo = transform.position;

        GetMap();
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        Debug.DrawRay(transform.position, transform.right, Color.green);
        Debug.DrawRay(transform.position, -transform.right, Color.green);

        if (transform.position == posToGo)
            DrawWay();
        else
            GoToPos();
    }
    private void GetMap()
    {
        point _p = new point();
        _p.pos = new Vector2(transform.position.x, transform.position.z);
        Map = Navigation.DrawMap(_p);
    }
    private void GoToPos()
    {
        transform.position = Vector3.MoveTowards(transform.position, posToGo, .01f);
        transform.LookAt(posToGo);
        if (toRotate != 0)
        {
            transform.Rotate(Vector3.up, toRotate);
            toRotate = 0;
        }
    }
    private bool ChangeDirection()
    {
        RaycastHit hit;

        if (Physics.Linecast(transform.position, transform.position + transform.forward * 2f, out hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                return true;
            }
        }

        return false;
    }
    private void DrawWay()
    {
        if (GameManager.instance.keys.Count > 0
            && transform.position == GameManager.instance.keys[0].transform.position)
        {
            Destroy(GameManager.instance.keys[0]);
            GameManager.instance.keys.Remove(GameManager.instance.keys[0]);
            GameManager.instance.curKeys++;
        }
        if (!Navigation.isBusy)
        {
            if (Way != null && Way.Count > 0 && !ChangeDirection())
            {
                posToGo = new Vector3(Way[0].pos.x, .5f, Way[0].pos.y);
                Way.Remove(Way[0]);
                isReady = true;
            }
            else if ((Way == null || Way.Count == 0) && isReady)
            {
                isReady = false;
                Way = null;
                point pointToGo;
                Vector2 pos2;

                if (GameManager.instance.keys.Count > 0)
                {
                    pos2 = new Vector2(GameManager.instance.keys[0].transform.position.x, GameManager.instance.keys[0].transform.position.z);
                    pointToGo = Map.Find(p => p.pos == pos2);
                }
                else
                    pointToGo = Map[Random.Range(0, Map.Count)];

                if (new Vector3(pointToGo.pos.x, transform.position.y, pointToGo.pos.y) != transform.position)
                {
                    Way = Navigation.DrawWay(Map,
                        new Vector2(transform.position.x, transform.position.z),
                        pointToGo.pos);
                }
            }
        }
    }
}
