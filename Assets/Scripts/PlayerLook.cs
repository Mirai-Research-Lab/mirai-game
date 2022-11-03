using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cam;
    private float xRotation = 0f;
    [SerializeField] private float xSensitivity = 30f;
    [SerializeField] private float ySensitivity = 30f; 

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        //calculate camera rotation 
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        //apply rotation to camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //apply rotation to body
        transform.Rotate(Vector3.up * mouseX * Time.deltaTime * xSensitivity);
    }
}
