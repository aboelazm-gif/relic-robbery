using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimStateManager : MonoBehaviour
{
    public Cinemachine.AxisState xAxis, yAxis;
    public Transform cameraPoint;
    public Animator animator;
    AnimatorStateInfo state;
    bool locked = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (gameObject.GetComponent<MovementScript>().dead) return;
        state = animator.GetCurrentAnimatorStateInfo(0);
        locked = state.IsName("Take Item");
        if (!locked)
        {
            xAxis.Update(Time.deltaTime);
            yAxis.Update(Time.deltaTime);
        }
    }
    private void LateUpdate()
    {
        if (!locked)
        {
            cameraPoint.localEulerAngles = new Vector3(yAxis.Value, cameraPoint.localEulerAngles.y, cameraPoint.localEulerAngles.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
        }
    }
}
