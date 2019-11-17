using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoomBoundsSize : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(Mathf.Round(Mathf.Max(16 * 2, this.transform.localScale.x)), Mathf.Round(Mathf.Max(9 * 2, this.transform.localScale.y)));//.Clamp(new Vector3(19*3, 7*3, 1), new Vector3(int.MaxValue, int.MaxValue, 1));
        this.transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
    }
}
