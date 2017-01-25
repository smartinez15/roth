using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{

    public Transform target;            // The position that that camera will be following.
    public float smooth = 3f;        // The speed with which the camera will be following.
    public float rotationSmooth = 4f;

    private Vector3 offset;                     // The initial offset from the target.
    private Quaternion rot;
    private bool rotate;

    void Start()
    {
        // Calculate the initial offset.
        offset = transform.position - target.position;
        rotate = false;

        rot = transform.localRotation;
    }

    void FixedUpdate()
    {
        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = target.position + offset;

        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smooth * Time.deltaTime);

        if(Input.GetKey(KeyCode.Q))
        {
            Vector3 eul = rot.eulerAngles;
            rot = Quaternion.Euler(eul.x, eul.y + 2.5f, eul.z);
            rotate = true;
        }
        if(Input.GetKey(KeyCode.E))
        {
            Vector3 eul = rot.eulerAngles;
            rot = Quaternion.Euler(eul.x, eul.y - 2.5f, eul.z);
            rotate = true;
        }

        if (rotate)
        {
            float delta = rot.eulerAngles.y;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, rot, rotationSmooth * Time.deltaTime);
            delta -= transform.localEulerAngles.y;
            if(Mathf.Abs(delta)<0.2f)
            {
                rotate = false;
                transform.localRotation = rot;
            }
        }
    }
}
