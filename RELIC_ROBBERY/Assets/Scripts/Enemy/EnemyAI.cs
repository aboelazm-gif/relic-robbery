using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] patrolPoints;
    public Transform Guard;
    public int targetPoint;
    public float moveSpeed = 3f;
    public float turnSpeed = 400f;
    public int maxHealth = 3;

    public UnityEngine.UI.Image alertBar;

    int health;
    [HideInInspector] public float alertLevel = 0f;
    public float alertRate = 60f;
    public float alertDropoffRate = 30f;
    public float alertRange = 3f;
    public float gravity = -9.81f;
    float distanceToTarget;
    public Animator animator;
    public Transform player;

    public CharacterController controller;

    public Transform groundCheck;
    public Transform flashlight;
    public float groundDistance = 0.4f;
    Vector3 fallVelocity;

    float punchTimer = 0f;
    float attackRange = 0.7f;
    [HideInInspector] public bool playerTooClose = false;

    private bool isWaiting;
    bool isPunching = false;

    bool alert = false;

    bool patrolling = true;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Guard.position, alertRange);
    }
    void Start()
    {
        isWaiting = false;
        health = maxHealth;
    }

    public bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, LayerMask.GetMask("Ground"));
    }
    public void takeDamage()
    {
        health--;
        alert = true;
        animator.SetBool("alert", true);
        patrolling = false;
        if (health > 0)
        {
            int hitAnimation = Random.Range(0, 2);
            switch (hitAnimation)
            {
                case 0: animator.SetTrigger("Hit1"); break;
                case 1: animator.SetTrigger("Hit2"); break;
            }
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    public void setAlert(float alert)
    {
        alertLevel = alert;
        alertBar.fillAmount = alertLevel / 100f;
    }
    void Update()
    {
        if (player.GetComponent<MovementScript>().dead)
        {
            alert = false;
            animator.SetBool("alert", false);
            return;
        }
        playerTooClose = Physics.OverlapSphere(Guard.position, alertRange, LayerMask.GetMask("Player")).Length > 0 && !player.GetComponent<MovementScript>().isCrouching;
        if (playerTooClose)
        {
            setAlert(alertLevel + Time.deltaTime * alertRate * 5);
        }
        if (alertLevel > 100f)
        {
            animator.SetBool("alert", true);
            alert = true;
            patrolling = false;
            alertBar.enabled = false;
        }
        else
        {
            alert = false;
            if (alertLevel > 0.5f)
            {
                alertBar.enabled = true;
                alertBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled = true;

            }
            else
            {
                alertBar.enabled = false;
                alertBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punch 1") || animator.GetCurrentAnimatorStateInfo(0).IsName("Punch 2"))
        {
            isPunching = true;

        }
        else
        {
            isPunching = false;

        }
        if (isGrounded() && fallVelocity.y < 0)
        {
            fallVelocity.y = -2f;
        }
        fallVelocity.y += gravity * Time.deltaTime;
        controller.Move(fallVelocity * Time.deltaTime);

        if (!isWaiting && patrolling && !alert)
        {
            distanceToTarget = Vector3.Distance(Guard.transform.position, patrolPoints[targetPoint].position);
            if (distanceToTarget > 0.5f)
            {
                Vector3 direction = (patrolPoints[targetPoint].position - Guard.transform.position).normalized;
                animator.SetBool("walk", true);
                controller.Move(direction * moveSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("walk", false);
                StartCoroutine(WaitAndMoveToNextPoint());
            }
        }
        if (alert)
        {
            moveSpeed = 5f;
            if (transform.Find("Guy/cone")) Destroy(transform.Find("Guy/cone").gameObject);
            if (flashlight) Destroy(flashlight.gameObject);
            if (!isPunching)
            {
                Vector3 direction = player.position - Guard.position;
                direction.y = 0;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                float distanceToTarget = Vector3.Distance(Guard.position, player.position);
                if (distanceToTarget > 1.1f)
                {
                    animator.SetBool("walk", true);
                    controller.Move(direction.normalized * moveSpeed * Time.deltaTime);
                }
                else
                {
                    animator.SetBool("walk", false);
                }

                Guard.rotation = Quaternion.RotateTowards(Guard.rotation, lookRotation, turnSpeed * Time.deltaTime);
                if (!animator.GetBool("walk") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hit1") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hit2")) punchTimer += Time.deltaTime;

                if (punchTimer > 2f) Punch();
            }
        }
    }

    void Punch()
    {
        int punchAnimation = Random.Range(0, 2);
        switch (punchAnimation)
        {
            case 0: animator.SetTrigger("Punch1"); break;
            case 1: animator.SetTrigger("Punch2"); break;
        }
        animator.SetBool("walk", false);
        punchTimer = 0;
        StartCoroutine(DelayAction(0.5f));

    }

    IEnumerator WaitAndMoveToNextPoint()
    {
        isWaiting = true;

        yield return new WaitForSeconds(3f);

        // Rotate only on the y-axis
        Vector3 direction = patrolPoints[targetPoint].position - Guard.position;
        direction.y = 0;  // Keep y component zero to rotate only on the y-axis
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Reverse the direction he's facing if the target point is 1, else rotate normally
        if (targetPoint == 1)
        {
            lookRotation *= Quaternion.Euler(0, -180, 0);
        }
        else
        {
            lookRotation *= Quaternion.Euler(0, 180, 0);
        }

        // Rotate with the specified turn speed
        while (Quaternion.Angle(Guard.rotation, lookRotation) > 0.1f)
        {
            Guard.rotation = Quaternion.RotateTowards(Guard.rotation, lookRotation, turnSpeed * Time.deltaTime);
            yield return null;
        }

        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
        isWaiting = false;
    }
    IEnumerator DelayAction(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        Collider[] collidersInSphere = Physics.OverlapSphere(Guard.position + new Vector3(0, 1, 0) + Guard.forward * attackRange, 1f, LayerMask.GetMask("Player"));
        if (collidersInSphere.Length > 0)
        {

            collidersInSphere[0].gameObject.GetComponentInParent<PlayerHealth>().takeDamage();
        }
        //Do the action after the delay time has finished.
    }
}
