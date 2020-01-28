using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MainTilemap : MonoBehaviour
{
    public static MainTilemap Instances;
    Dictionary<string, Tilemap> Layers = new Dictionary<string, Tilemap>();
    
    // Start is called before the first frame update
    void Start()
    {
        Instances = this;
        Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();
        foreach(Tilemap tilemap in tilemaps)
        {
            Layers.Add(tilemap.gameObject.name, tilemap);
        }
    }

    
    public void AddTilemapToLayer(Tilemap tilemap, string layer)
    {
        BoundsInt bounds = tilemap.cellBounds;
        try
        {
            Tilemap targetTilemap = Layers[layer];

            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    Vector3Int cellpos = tilemap.WorldToCell(tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)) + tilemap.transform.position);
                    TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));

                    if (tile != null)
                    {
                        targetTilemap.SetTile(cellpos, tile);
                    }
                }
            }
        } catch (KeyNotFoundException e)
        {
            Debug.Log("Your tilemap can not be merged with main tilemap, because it's name does not match with any available main tilemap layer. Did you forget to add the TilemapLayer component to your tilemap?");
        }
    }

    public void Merge()
    {
        Instances = this;
        Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tilemap in tilemaps)
        {
            Layers.Add(tilemap.gameObject.name, tilemap);
        }
        TilemapLayer[] tilemaplayers = Object.FindObjectsOfType<TilemapLayer>();
        foreach(TilemapLayer layer in tilemaplayers)
        {
            Debug.Log(layer.GetComponent<Tilemap>() + " " + layer.gameObject.name);
            AddTilemapToLayer(layer.GetComponent<Tilemap>(), layer.gameObject.name);
        }
    }
}
