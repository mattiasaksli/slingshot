using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoomBoundsSize : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(Mathf.Round(Mathf.Max(15 * 2, this.transform.localScale.x)), Mathf.Round(Mathf.Max(8.4375f * 2, this.transform.localScale.y)));//.Clamp(new Vector3(19*3, 7*3, 1), new Vector3(int.MaxValue, int.MaxValue, 1));
        //this.transform.localScale = new Vector3(this.transform.localScale.x-this.transform.localScale.x % 30, this.transform.localScale.y-this.transform.localScale.y % 16.875f);
        //this.transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
    }
}
