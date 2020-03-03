﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoorPlayerController : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = Vector3.zero;

        velocity.x += Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        velocity.z += Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.position += velocity;
    }
}