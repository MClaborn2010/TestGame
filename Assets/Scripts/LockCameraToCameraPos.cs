using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockCameraToCameraPos : MonoBehaviour
{
    [Header("Camera Position")]
    public Transform cameraPosition;

    void Update()
    {
        transform.position = cameraPosition.position; // Assigns transform.position of camera to the Camera Position game object. 
    }
}
