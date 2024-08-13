using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float xSensitivity = 30f;
    [SerializeField] private float ySensitivity = 30f;

    private float xRotation;

    public void ProcessLook(Vector2 input) 
    {
        //Note : when we say rotaion inx-axis it means up & down - rotation is diff than usual movement
        // and roation in y-axis means left-right 
        float mouseX = input.x;
        float mouseY = input.y;

        //Vertical Rotation (xRotation): The camera's vertical rotation is adjusted by subtracting the vertical mouse movement (mouseY)
        //multiplied by Time.deltaTime and ySensitivity. This makes the camera look up or down smoothly and at a controlled speed.

        //If you move the mouse up, mouseY is positive, so you subtract a positive number from xRotation.
        //This makes xRotation smaller, which means the camera looks down.

        //If you move the mouse down, mouseY is negative, so you subtract a negative number(which is like adding).
        //This makes xRotation larger, which means the camera looks up.

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        //clamping rotation in y-axis from -80 to 80
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        //rotating camera
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //Horizontal Rotation (transform.Rotate): The player object is rotated around the y-axis by the horizontal mouse movement (mouseX) multiplied by Time.deltaTime and xSensitivity.
        //This makes the player look left or right smoothly and at a controlled speed.
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
