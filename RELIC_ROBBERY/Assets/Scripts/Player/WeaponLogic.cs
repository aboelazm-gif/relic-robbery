using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    bool armed = false;
    public GameObject weaponContainer;
    GameObject currentWeapon;


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

    }
}
