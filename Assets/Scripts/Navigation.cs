using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public static bool isBusy;
    public static List<point> DrawMap(point startPoint)
    {
        isBusy = true;
        Debug.Log("Started drawing");

        List<point> _list = new List<point>();
        int stopper = 0;
        Vector3 offset = new Vector3(0, .25f, 0);
        bool isDrawing = false;

        void DrawPoses(Vector3 pos)
        {
            isDrawing = true;
            Debug.Log("Drawing 4 poses");

            for (int i = 0; i < 4; i++)
            {
                point _p = new point();
                Vector3 _dir = i switch
                {
                    0 => Vector3.forward,
                    1 => -Vector3.forward,
                    2 => Vector3.right,
                    _ => -Vector3.right,
                };
                if (!CheckCollision("Wall", pos + offset, _dir + offset))
                {
                    Vector2 _setPos = new Vector2(pos.x + _dir.x, pos.z + _dir.z);
                    _p.pos = _setPos;
                    if (!_list.Exists(p => p.pos == _p.pos)) _list.Add(_p);
                }
            }
            Debug.Log("Drew");
            isDrawing = false;
        }

        while (stopper < 100000)
        {
            Debug.Log("Entering cycle");
            stopper++;

            if (_list.Count == 0)
            {
                Debug.Log("First iteration");
                startPoint.isChecked = true;
                _list.Add(startPoint);
                DrawPoses(new Vector3(startPoint.pos.x, 0, startPoint.pos.y));
            }

            point _p = _list.Find(p => p.isChecked == false);
            if (!isDrawing && _p != null)
            {
                Debug.Log("Iteration");
                _p.isChecked = true;
                DrawPoses(new Vector3(_p.pos.x, 0, _p.pos.y));
            }
            else if (!isDrawing) break;
        }

        isBusy = false;
        return _list;
    }
    public static bool CheckCollision(string tag, Vector3 pos, Vector3 dir)
    {
        RaycastHit hit;

        if (Physics.Linecast(pos, pos + dir, out hit))
        {
            if (hit.collider.CompareTag(tag)) return true;
        }

        return false;
    }
    public static List<point> DrawWay(List<point> map, Vector2 start, Vector2 goal)
    {
        isBusy = true;

        List<point> _openSet = new List<point>();
        List<point> _closedSet = new List<point>();
        int stopper = 0;
        bool isReady = true;

        List<point> FindNeighbours(point _point)
        {
            isReady = false;
            List<point> neighbours = new List<point>();

            for (int i = 0; i < 4; i++)
            {
                point _p = new point();
                Vector2 _dir = i switch
                {
                    0 => Vector2.up,
                    1 => Vector2.down,
                    2 => Vector2.right,
                    _ => Vector2.left,
                };
                _p.pos = _point.pos + _dir;
                if (map.Exists(p => p.pos == _p.pos))
                {
                    _p.CameFrom = _point;
                    _p.G = _point.G + 1;
                    _p.H = GetDistance(_p.pos, goal);
                    _openSet.Remove(_openSet.Find(p => p.pos == _p.pos));
                    neighbours.Add(_p);
                }
            }
            isReady = true;

            if (neighbours.Count != 0) return neighbours;
            else return null;
        }
        List<point> GetPath(point _point)
        {
            var way = new List<point>();
            var currentPoint = _point;

            while (currentPoint != null)
            {
                way.Add(currentPoint);
                currentPoint = currentPoint.CameFrom;
            }
            way.Reverse();
            return way;
        }

        point startPoint = new point()
        {
            CameFrom = null,
            pos = start,
            G = 0,
            H = GetDistance(start, goal)
        };
        _openSet.Add(startPoint);
        while (_openSet.Count > 0)
        {
            point currentPoint = _openSet.OrderBy(p => p.F).First();

            if (currentPoint.pos == goal)
            {
                isBusy = false;
                return GetPath(currentPoint);
            } 

            _openSet.Remove(currentPoint);
            _closedSet.Add(currentPoint);

            foreach (point neighbourPoint in FindNeighbours(currentPoint))
            {
                if (_closedSet.Count(_p => _p.pos == neighbourPoint.pos) > 0)
                    continue;
                point openNode = _openSet.FirstOrDefault(_p =>
                  _p.pos == neighbourPoint.pos);

                if (openNode == null)
                    _openSet.Add(neighbourPoint);
                else
                  if (openNode.G > neighbourPoint.G)
                {
                    openNode.CameFrom = currentPoint;
                    openNode.G = neighbourPoint.G;
                }
            }
        }

        isBusy = false;
        return null;
    }
    public static float GetDistance(Vector2 from, Vector2 to)
    {
        return Mathf.Abs(from.x - to.x) + 
            Mathf.Abs(from.y - to.y);
    }
}
public class point
{
    public Vector2 pos { get; set; }
    public point CameFrom { get; set; }
    public float G { get; set; } //Distance from starting point
    public float H { get; set; } //Estimated distance
    public float F { get { return this.H + this.G; } }

    public bool isChecked { get; set; }
}
