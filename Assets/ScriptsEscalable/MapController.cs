using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [System.Serializable]
    public class Map
    {
        public WallController.Wall[] walls;

        //
        // lights...
        // [...]
        //
    }

    public Map map;

    [SerializeField] private Transform _wallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        name = "map";
        // for this simple version we will only focus on walls,
        // in the real project we would handle everything on the map, like the lights or the spots

        //GameObject walls = GameObject.Find("Walls");
        foreach (WallController.Wall wall in map.walls)
        {
            Instantiate(_wallPrefab, transform)
                .gameObject
                .GetComponent<WallController>()
                .wall = wall;
        }
    }

}
