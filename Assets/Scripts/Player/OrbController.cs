﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{
    public OrbBody body;
    public float AccelerationGround = 40;
    public float AccelerationAir = 30;
    public float GravityPower = 40;
    public bool IsFloating = true;
    [HideInInspector]
    public SuperBooster isSuperBoosting = null;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<OrbBody>();
    }

    void FixedUpdate()
    {
        if(IsFloating)
        {
            body.TargetMovement.x = 0;
            body.TargetMovement.y = 0;
            body.Acceleration = AccelerationAir;
        } else
        {
            body.Movement.y -= GravityPower * Time.deltaTime;
            body.TargetMovement.y = body.Movement.y;
            body.Acceleration = AccelerationAir;
        }
        body.Move(body.Movement*Time.deltaTime);
    }
}
