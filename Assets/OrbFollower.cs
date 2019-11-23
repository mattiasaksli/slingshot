using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbFollower : MonoBehaviour
{
    public Transform Orb;
    public Vector3 TargetPosition;
    public bool Available;

    private PlayerController player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
