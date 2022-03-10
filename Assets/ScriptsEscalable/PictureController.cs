using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PictureController : MonoBehaviour
{
    [System.Serializable]
    public class Picture
    {
        public string id;
        public Vector3 startFrom;
        public float width;
        public float height;
        public int depth;
        public string url;
        //    label;
        public int has_frame;
        //    frame;
        public string edge_color;
        public string side;
        //    userData;



    }

    public Picture picture;

    [SerializeField] private GameObject _cube;
    [SerializeField] private GameObject _quad;
    private WallController parentWall;

    // Start is called before the first frame update
    void Start()
    {
        name = picture.id;
        parentWall = transform.parent.GetComponent<WallController>();
        StartCoroutine(GetTexture());
    }
    IEnumerator GetTexture()
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(picture.url);
        yield return webRequest.SendWebRequest();

        _quad.GetComponent<Renderer>().material.mainTexture = DownloadHandlerTexture.GetContent(webRequest);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 side = new Vector3();
        Vector3 quadPosition = new Vector3();
        if (picture.side == "right")
        {
            side.z = parentWall.wall.thickness / 2 + picture.depth / 2;
            quadPosition.z = picture.depth / 2 + 0.1f;
        }
        else
        {
            side.z = -parentWall.wall.thickness / 2 - picture.depth / 2;
            quadPosition.z = -picture.depth / 2 - 0.1f;
        }

        // we use position instead of local position because we want to count the rotation of the parent as well
        transform.position = transform.parent.transform.position + picture.startFrom;
        transform.localPosition += side;

        _cube.transform.localScale = new Vector3(picture.width, picture.height, picture.depth);


        _quad.transform.localScale = new Vector3(picture.width, picture.height, picture.side == "right" ? -1 : 1);
        _quad.transform.localPosition = quadPosition;
    }
}
