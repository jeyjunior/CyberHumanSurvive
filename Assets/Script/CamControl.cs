using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{

    float xRotation = 0.0f;
    float yRotation = 0.0f;

    public float mouseSensitivity = 400f;
    public float maxLookAngle = 60;
    public Transform orientation;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        FPSCamControl();
    }

    void FPSCamControl()
    {
        yRotation += Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;
        xRotation -= Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
