using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileWalls : MonoBehaviour
{
    public GameObject wallPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if(tile != null)
                {
                    GameObject wall = GameObject.Instantiate<GameObject>(wallPrefab,gameObject.transform);
                    wall.transform.position = new Vector3(x+bounds.xMin+0.5f, y+bounds.yMin+0.5f, 0);
                }
            }
        }
    }
}
