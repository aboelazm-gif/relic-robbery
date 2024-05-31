using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    bool armed = false;
    public GameObject weaponContainer;
    GameObject currentWeapon;
    bool canInteract;
    public Animator animator;
    public float attackRange = 1.5f;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void changeWeapon(GameObject newWeapon)
    {
        if (!armed) armed = true;
        if (currentWeapon)
        {
            Destroy(currentWeapon);
        }
        currentWeapon = Instantiate(newWeapon, weaponContainer.transform.position, weaponContainer.transform.rotation);
        currentWeapon.transform.parent = weaponContainer.transform;
        currentWeapon.transform.localPosition = newWeapon.transform.position;
        currentWeapon.transform.localRotation = newWeapon.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        canInteract = GetComponent<ObjectInteract>().canInteract;
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isGrounded = GetComponent<MovementScript>().isGrounded();
        bool locked = state.IsName("Take Item") || state.IsName("Stab 1") || state.IsName("Stab 2") || state.IsName("Landing") || state.IsName("Crouching") || state.IsName("Jumping") || GetComponent<MovementScript>().isCrouching;
        if (armed)
        {
            if (Input.GetMouseButtonDown(0) && !canInteract && !locked && isGrounded)
            {
                Attack();
            }
        }

    }
    void Attack()
    {
        int animation = Random.Range(0, 2);
        switch (animation)
        {
            case 0: animator.SetTrigger("Stab1"); break;
            case 1: animator.SetTrigger("Stab2"); break;
        }
        Delay(0.5f);

    }
    void Delay(float delayTime)
    {
        StartCoroutine(DelayAction(delayTime));
    }
    IEnumerator DelayAction(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        Collider[] collidersInSphere = Physics.OverlapSphere(transform.position + transform.forward * attackRange, 2f, LayerMask.GetMask("Enemy"));
        if (collidersInSphere.Length > 0)
        {
            Debug.Log("Hit");
            foreach (Collider collider in collidersInSphere)
            {
                collider.gameObject.GetComponentInParent<EnemyAI>().takeDamage();
            }
        }

        //Do the action after the delay time has finished.
    }
}
