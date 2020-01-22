using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : PlatformController
{
    public bool triggered;
    public bool StartTime;
    private float StartTimestamp;
    public float FallSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Triggered()
    {
        if(!triggered)
        {
            triggered = true;

        }
    }
}
