using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileOutsideRenderer : MonoBehaviour
{
    public RoomBoundsManager room;

    // Start is called before the first frame update
    void Start()
    {
        if (room)
        {
            Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();


            Bounds roombounds = room.GetComponent<BoxCollider2D>().bounds;
            foreach (Tilemap tilemap in tilemaps)
            { 
                //Debug.Log(roombounds + ": " + roombounds.min + ", " + roombounds.max);

                BoundsInt bounds = tilemap.cellBounds;

                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    for (int y = bounds.min.y; y < bounds.max.y; y++)
                    {
                        Vector3 cellpos = tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                        float worldx = cellpos.x;
                        float worldy = cellpos.y;
                        //Debug.Log(cellpos + new Vector3(-0.25f, -0.25f, 0));
                        if (worldx + 0.25f <= roombounds.min.x || worldx - 0.25f >= roombounds.max.x || worldy + 0.25f <= roombounds.min.y || worldy - 0.25f >= roombounds.max.y)
                        {
                            tilemap.SetColor(new Vector3Int(x, y, 0), Color.clear);
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
