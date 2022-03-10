using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [System.Serializable]
    public class Wall
    {
        public string name;
        public Vector3 startFrom;
        public int width;
        public int height;
        public int thickness;
        public int angle;
        public int has_texture;
        public string texture;
        public string color;
        public PictureController.Picture[] pictures;
    }

    public Wall wall;

    [SerializeField] private GameObject _cube;
    [SerializeField] private Transform _picturePrefab;

    // Start is called before the first frame update
    void Start()
    {
        name = wall.name;

        foreach (PictureController.Picture picture in wall.pictures)
        {

            Instantiate(_picturePrefab, transform)
                .gameObject
                .GetComponent<PictureController>()
                .picture = picture;

        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(
            new Vector3(-wall.startFrom.x, wall.startFrom.y, wall.startFrom.z),
            Quaternion.Euler(0, 360 - wall.angle, 0));

        _cube.transform.localScale = new Vector3(wall.width, wall.height, wall.thickness);
    }
}
