using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 5;
    int health;
    public Animator animator;
    public Slider healthBar;
    void Start()
    {
        health = maxHealth;
        healthBar.value = health;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void takeDamage()
    {
        if (gameObject.GetComponent<MovementScript>().dead) return;
        int hitAnimation = Random.Range(0, 2);
        health--;
        healthBar.value = (float)health / maxHealth;
        if (health <= 0) gameObject.GetComponent<MovementScript>().Die();
        else switch (hitAnimation)
            {
                case 0: animator.SetTrigger("Hit1"); break;
                case 1: animator.SetTrigger("Hit2"); break;
            }
    }
}
