﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EnableDepth : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
