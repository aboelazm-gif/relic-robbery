using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float sizeFactor = 0.012f; // Adjust this value to set the desired size

    public bool scaledScreenSize = false;
    public float minimumScale = 0;

    public float maximumScale = 1000;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        // Make the billboard face the camera
        transform.LookAt(transform.position + cam.forward);

        if (scaledScreenSize)
        {
            float distance = Vector3.Distance(transform.position, cam.position);
            float scale = Mathf.Clamp(distance * sizeFactor, minimumScale, maximumScale);
            transform.localScale = new Vector3(scale, scale, scale);
        }


    }
}
