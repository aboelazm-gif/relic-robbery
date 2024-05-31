using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 5;
    public Animator animator;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void takeDamage()
    {
        int hitAnimation = Random.Range(0, 2);
        maxHealth--;
        switch (hitAnimation)
        {
            case 0: animator.SetTrigger("Hit1"); break;
            case 1: animator.SetTrigger("Hit2"); break;
        }
    }
}
