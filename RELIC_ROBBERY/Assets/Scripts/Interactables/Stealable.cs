using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealable : MonoBehaviour, IInteractables
{
    // Start is called before the first frame update
    public GameObject weaponPrefab;
    GameObject Player;
    public int stealValue = 1000;
    void Start()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    GameObject FindChildWithTag(GameObject parent, string tag)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }
    void Update()
    {

    }
    public void Interact()
    {
        //Only steals for now. Can be expanded to other interactions
        Steal();
    }
    public void Steal()
    {
        GameObject weapon = FindChildWithTag(gameObject, "Weapon"); //If the interacted object has a child with the tag "Weapon", it will be stored in the weapon variable
        if (weapon && weaponPrefab)
        {
            Player.GetComponent<WeaponLogic>().changeWeapon(weaponPrefab);

        }
        Player.GetComponent<ObjectInteract>().score += stealValue;
        Debug.Log(Player.GetComponent<ObjectInteract>().score);
        Destroy(gameObject);
    }
}
