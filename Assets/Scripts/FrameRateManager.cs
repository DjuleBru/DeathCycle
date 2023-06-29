using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    void Start() {
        // Limit the framerate to 60
        Application.targetFrameRate = 60;
    }
}
