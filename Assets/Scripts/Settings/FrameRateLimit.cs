using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateLimit : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }
}
