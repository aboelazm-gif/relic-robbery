using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteract : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Camera;
    public Image Cursor;
    public Sprite Crosshair;
    public Sprite Interact;
    public float interactDistance = 5f;

    [HideInInspector] public int score = 0;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.position, Camera.forward, out hit, interactDistance))
        {

            if (hit.collider.tag == "Interactable" || hit.collider.tag == "Stealable")
            {
                Cursor.sprite = Interact;
                if (Input.GetButtonDown("Fire1"))
                {
                    hit.collider.GetComponent<Interactable>().Interact();
                }
            }
            else
            {
                Cursor.sprite = Crosshair;
            }
        }
        else
        {
            Cursor.sprite = Crosshair;
        }
    }
}
