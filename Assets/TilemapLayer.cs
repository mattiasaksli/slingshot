using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapLayer : MonoBehaviour
{

    private void Update()
    {
        if (MainTilemap.Instances) {
            MainTilemap.Instances.AddTilemapToLayer(GetComponent<Tilemap>(), gameObject.name);
            GameObject.Destroy(gameObject);
        }
    }
}
