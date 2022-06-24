using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWalk : MonoBehaviour
{
    [Header("DEBUG")]
    public GameObject mapPoints;
    public Transform mp_Parent;
    public GameObject wayPoints;
    public Transform wp_Parent;

    public Vector3 destination;

    bool isReady = true;

    private List<point> Map;
    private List<point> Way;

    float toRotate;
    Vector3 posToGo;

    private void Awake()
    {
        posToGo = transform.position;

        GetMap();
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        Debug.DrawRay(transform.position, transform.right, Color.green);
        Debug.DrawRay(transform.position, -transform.right, Color.green);

        if (transform.position == posToGo)
            DrawWay();
        else
            GoToPos();

        DebugInput();
    }
    private void DebugInput()
    {
        if (Input.GetKeyUp(KeyCode.F1)) // Draw or Clear map
        {
            if (mp_Parent.childCount > 0)
                foreach (Transform child in mp_Parent) Destroy(child.gameObject);
            else
                foreach (point _p in Map) Instantiate(mapPoints, new Vector3(_p.pos.x, 0, _p.pos.y), Quaternion.identity, mp_Parent);
        }
        if (Input.GetKeyUp(KeyCode.F2)) // Draw or Clear current way
        {
            if (wp_Parent.childCount > 0)
                foreach (Transform child in wp_Parent) Destroy(child.gameObject);
            else
                foreach (point _p in Way) Instantiate(wayPoints, new Vector3(_p.pos.x, 0, _p.pos.y), Quaternion.identity, wp_Parent);
        }
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
    private void DrawWay()
    {
        if (!Navigation.isBusy)
        {
            if (Way != null && Way.Count > 0)
            {
                posToGo = new Vector3(Way[0].pos.x, .5f, Way[0].pos.y);
                Way.Remove(Way[0]);
                isReady = true;
            }
            else if ((Way == null || Way.Count == 0) && isReady)
            {
                isReady = false;
                Way = null;

                point pointToGo = Map[Random.Range(0, Map.Count)];

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
