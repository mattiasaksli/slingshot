﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetScrolling : MonoBehaviour
{
    // Scroll main texture based on time

    float scrollSpeed = 0.5f;
    Renderer rend;
    private Vector2 scrolling;
    public Vector2 ScrollingSpeed;
    Camera camera;
    private Vector3 localPositon;

    void Start()
    {
        rend = GetComponent<Renderer>();
        camera = Camera.main;
        localPositon = camera.transform.position;
    }

    void FixedUpdate()
    {
        localPositon = Vector3.Lerp(localPositon, camera.transform.position, 1);
        scrolling = localPositon;
        scrolling.x *= ScrollingSpeed.x;
        scrolling.y *= ScrollingSpeed.y;
        rend.material.SetVector("_Scrolling", scrolling);
    }
}
