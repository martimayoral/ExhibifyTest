using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExpositionController : MonoBehaviour
{
    [SerializeField] TextAsset jsonExposition;

    [System.Serializable]
    public class Exposition
    {
        //
        // name...
        // has_floor...
        // [...]
        // 

        public MapController.Map map;

        // [...]
    }

    public Exposition exposition;

    [SerializeField] private Transform _mapPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // in this version the json is given through a TextAsset file,
        // in the real version it would be given through the server API
        exposition = JsonUtility.FromJson<Exposition>(jsonExposition.text);

        // add the map to the scene with the component map controller,
        // so it initiates all the objects of the scene.
        Instantiate(_mapPrefab, transform)
            .gameObject
            .GetComponent<MapController>()
            .map = exposition.map;

        Debug.Log(JsonUtility.ToJson(exposition, true));
    }

}
