using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour, IInteractables
{
    // Start is called before the first frame update
    public GameObject Hologram;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        animator.SetTrigger("click");
        Hologram.SetActive(true);
    }
}
