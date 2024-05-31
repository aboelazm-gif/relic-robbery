using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform CameraPoint;
    public float moveSpeed = 6f;
    public float sprintMultiplier = 1.5f;
    public float crouchMultiplier = 0.5f;
    public float jumpHeight = 3f;
    public float transitionSmoothing = 3f;
    public Transform groundCheck;
    public float gravity = -12f;
    public float groundDistance = 0.4f;
    public Animator animator;


    bool isSprinting = false;
    bool isMoving = false;
    [HideInInspector] public bool isCrouching = false;
    bool stealth = false;

    float fallTimer = 0;

    float targetHeight;
    Vector3 targetCenter;
    Vector3 targetCameraPosition;

    float speedMultiplier()
    {
        if (isCrouching) return crouchMultiplier;
        if (isSprinting) return sprintMultiplier;
        else return 1;
    }

    Vector3 fallVelocity;

    public bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, LayerMask.GetMask("Ground"));
    }

    void Start()
    {
        // Initialize the target height, center, and camera position with the initial values
        targetHeight = controller.height;
        targetCenter = controller.center;
        targetCameraPosition = CameraPoint.localPosition;
    }

    void Update()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Take Item")) return;
        //Movement
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit1") || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit2")) return;
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(moveX, 0, moveY).normalized;

        // Smooth transition for the moveY animator parameter
        float currentMoveY = animator.GetFloat("moveY");
        if (Mathf.Abs(moveY - currentMoveY) > 0.01f)
        {
            animator.SetFloat("moveY", Mathf.Lerp(currentMoveY, moveY, transitionSmoothing * Time.deltaTime));
        }
        else
        {
            animator.SetFloat("moveY", moveY);
        }

        // Smooth transition for the moveX animator parameter
        float currentMoveX = animator.GetFloat("moveX");
        isMoving = moveDirection.magnitude >= 0.1f;
        if (Mathf.Abs(moveX - currentMoveX) > 0.01f)
        {
            animator.SetFloat("moveX", Mathf.Lerp(currentMoveX, moveX, transitionSmoothing * Time.deltaTime));
        }
        else
        {
            animator.SetFloat("moveX", moveX);
        }

        if (isMoving) // Check if moving, use later for animations
        {
            animator.SetBool("walk", true);
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + CameraPoint.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * moveSpeed * speedMultiplier() * Time.deltaTime);
        }
        else
        {
            animator.SetBool("walk", false);
        }

        // Gravity
        if (fallTimer > 0.3f) animator.SetBool("airborne", true); // Check if the player has been falling for more than 0.3 seconds to avoid the fall animation when going down stairs
        if (isGrounded() && fallVelocity.y < 0)
        {
            fallVelocity.y = -2f;
            animator.SetBool("airborne", false);
        }
        if (!isGrounded()) fallTimer += Time.deltaTime;
        else
        {
            fallTimer = 0;
        }
        fallVelocity.y += gravity * Time.deltaTime;
        controller.Move(fallVelocity * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            animator.SetTrigger("Jump");
            animator.SetBool("airborne", true);
            fallVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }
        if (isSprinting && isMoving && !isCrouching) animator.SetBool("sprint", true);
        else animator.SetBool("sprint", false);

        // Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            animator.SetBool("crouch", true);

        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            animator.SetBool("crouch", false);

        }

        stealth = isCrouching && isGrounded();
        if (stealth)
        {
            targetHeight = 1.5f;
            targetCenter = new Vector3(0, 0, 0);
            targetCameraPosition = new Vector3(CameraPoint.localPosition.x, 0.3f, CameraPoint.localPosition.z);
        }
        else
        {
            targetHeight = 2.45f;
            targetCenter = new Vector3(0, 0.53f, 0);
            targetCameraPosition = new Vector3(CameraPoint.localPosition.x, 0.867f, CameraPoint.localPosition.z); // Adjust the stand height
        }
        // Smoothly interpolate the height and center of the controller
        controller.height = Mathf.Lerp(controller.height, targetHeight, transitionSmoothing * Time.deltaTime);
        controller.center = Vector3.Lerp(controller.center, targetCenter, transitionSmoothing * Time.deltaTime);

        // Smoothly interpolate the camera position
        CameraPoint.localPosition = Vector3.Lerp(CameraPoint.localPosition, targetCameraPosition, transitionSmoothing * Time.deltaTime);
    }
}
