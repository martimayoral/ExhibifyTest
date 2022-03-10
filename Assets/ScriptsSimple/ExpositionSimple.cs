using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class SPicture
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

[System.Serializable]
public class SWall
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
    public SPicture[] pictures;
}

[System.Serializable]
public class SMap
{
    public SWall[] walls;

    //
    // lights...
    // [...]
    //
}

[System.Serializable]
public class SExposition
{
    //
    // name...
    // has_floor...
    // [...]
    // 

    public SMap map;

    // [...]
}

public class ExpositionSimple : MonoBehaviour
{
    [SerializeField] TextAsset jsonExposition;


    // Start is called before the first frame update
    void Start()
    {
        // in this version the json is given through a TextAsset file,
        // in the real version it would be given through the server API
        SExposition exposition = JsonUtility.FromJson<SExposition>(jsonExposition.text);


        // in this SIMPLE version, walls will be added directly under the "Walls" 
        // game object.
        GameObject walls = GameObject.Find("Walls");

        // for each wall we do the following:
        foreach (SWall wall in exposition.map.walls)
        {
            // root game object for walls, set name and parent
            GameObject wallGameObject = new GameObject();
            wallGameObject.name = wall.name;
            wallGameObject.transform.SetParent(walls.transform);

            // set position. Json file uses right-handed coordination system,
            // but unity uses a left-handed coordinate system. That means that
            // the x axis is inverted, so we have to flip the x back.
            wallGameObject.transform.SetPositionAndRotation(
                new Vector3(-wall.startFrom.x, wall.startFrom.y, wall.startFrom.z),
                Quaternion.Euler(0, 360 - wall.angle, 0));

            // The wall itself will be represented by a cube in this SIMPLE version.
            // Instantiate, set parent and scale
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(wallGameObject.transform, false);
            cube.transform.localScale = new Vector3(wall.width, wall.height, wall.thickness);

            // For each picture in the wall we will do the following
            foreach (SPicture picture in wall.pictures)
            {
                // root game object for picture, set name and parent
                GameObject pictureGameObject = new GameObject();
                pictureGameObject.name = picture.id;
                pictureGameObject.transform.SetParent(wallGameObject.transform, false);

                // A cube for the volume and a Quad because it alows to
                // render a image using only 2 triangles so its faster
                GameObject pictureCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                pictureCube.transform.SetParent(pictureGameObject.transform, false);
                GameObject pictureQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                pictureQuad.transform.SetParent(pictureGameObject.transform, false);

                // the position of the picture depends on the side of the wall.
                // the z (how deep will it be) coordinate will be half of the thikness 
                // plus half of the depth of the picture, so its touching the wall
                Vector3 side = new Vector3();
                side.z = wall.thickness / 2 + picture.depth / 2;

                // the quad has to be right in the end of the cube
                Vector3 quadPosition = new Vector3();
                quadPosition.z = picture.depth / 2 + 0.1f;

                if (picture.side != "right")
                {
                    side *= -1;
                    quadPosition *= -1;
                }

                // root object
                // we use position instead of local position because we want to count the rotation of the parent as well
                pictureGameObject.transform.position = wallGameObject.transform.position + picture.startFrom;
                pictureGameObject.transform.localPosition += side;

                // scale of the cube
                pictureCube.transform.localScale = new Vector3(picture.width, picture.height, picture.depth);

                // scale and position of the quad
                pictureQuad.transform.localScale = new Vector3(picture.width, picture.height, picture.side == "right" ? -1 : 1);
                pictureQuad.transform.localPosition = quadPosition;

                // to get the image from a web server, given a url we can use the native Networking package
                StartCoroutine(GetTexture());

                IEnumerator GetTexture()
                {
                    UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(picture.url);
                    yield return webRequest.SendWebRequest();

                    pictureQuad.GetComponent<Renderer>().material.mainTexture = DownloadHandlerTexture.GetContent(webRequest);
                }
            }
        }

        Debug.Log(JsonUtility.ToJson(exposition, true));
    }

}
