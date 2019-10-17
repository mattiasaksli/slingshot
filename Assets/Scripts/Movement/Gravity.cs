using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float Power = 0.1f;
    private KinematicBody kinematicBody;
    
    // Start is called before the first frame update
    void Start()
    {
        kinematicBody = gameObject.GetComponent<KinematicBody>();
    }

    private void FixedUpdate()
    {
        kinematicBody.Movement.y -= Power * Time.deltaTime;
        kinematicBody.TargetMovement.y = kinematicBody.Movement.y;
    }
}
