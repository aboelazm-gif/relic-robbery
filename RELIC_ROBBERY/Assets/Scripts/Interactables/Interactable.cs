using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractables
{
    void Interact();
}

public class Interactable : MonoBehaviour
{
    GameObject Player;

    IInteractables interactableScript;



    // Start is called before the first frame update

    void Start()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        interactableScript = GetComponent<IInteractables>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Interact()
    {
        if (interactableScript != null) interactableScript.Interact();
        else Debug.LogError("No interactable script assigned.");
    }

}
