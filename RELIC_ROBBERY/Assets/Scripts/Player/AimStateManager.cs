using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimStateManager : MonoBehaviour
{
    public Cinemachine.AxisState xAxis, yAxis;
    public Transform cameraPoint;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }
    private void LateUpdate()
    {
        cameraPoint.localEulerAngles = new Vector3(yAxis.Value, cameraPoint.localEulerAngles.y, cameraPoint.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }
}
