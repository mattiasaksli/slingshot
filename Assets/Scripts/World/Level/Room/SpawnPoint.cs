using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.position + Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
