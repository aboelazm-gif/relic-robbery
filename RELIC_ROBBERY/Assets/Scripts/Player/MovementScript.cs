using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform Camera;
    public float moveSpeed = 6f;
    public float sprintMultiplier = 1.5f;
    public float jumpHeight = 3f;
    public float transitionSmoothing =3f;
    public Transform groundCheck;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public Animator animator;

    bool sprint(){
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    Vector3 fallVelocity;

    bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, LayerMask.GetMask("Ground"));

    }
    void Update()
    {
        //Movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0, moveY).normalized;
        if(moveY>animator.GetFloat("moveY")){
            animator.SetFloat("moveY",animator.GetFloat("moveY")+transitionSmoothing*Time.deltaTime);
        }
        if(moveY<animator.GetFloat("moveY")){
            animator.SetFloat("moveY",animator.GetFloat("moveY")-transitionSmoothing*Time.deltaTime);
        }
        if(moveX>animator.GetFloat("moveX")){
            animator.SetFloat("moveX",animator.GetFloat("moveX")+transitionSmoothing*Time.deltaTime);
        }
        if(moveX<animator.GetFloat("moveX")){
            animator.SetFloat("moveX",animator.GetFloat("moveX")-transitionSmoothing*Time.deltaTime);
        }
        if (moveDirection.magnitude >= 0.1f) //Check if moving, use later for animations
        {
            animator.SetBool("walk",true);
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
        }
        else{
            animator.SetBool("walk",false);
        }
        //Gravity
        if (isGrounded() && fallVelocity.y < 0)
        {
            fallVelocity.y = -2f;
        }
        fallVelocity.y += gravity * Time.deltaTime;
        controller.Move(fallVelocity * Time.deltaTime);
        //Jumping
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            fallVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
