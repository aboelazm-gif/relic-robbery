using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Transform coneOrigin;
    public Transform Player;
    public Transform Guard;

    float alertLevel;
    float alertRate;
    float alertDropoffRate;
    bool playerSeen = false;
    // Start is called before the first frame update
    void Start()
    {
        alertRate = Guard.GetComponent<EnemyAI>().alertRate;
        alertDropoffRate = Guard.GetComponent<EnemyAI>().alertDropoffRate;
    }

    // Update is called once per frame
    void Update()
    {
        alertLevel = Guard.GetComponent<EnemyAI>().alertLevel;
        if (playerSeen) Guard.GetComponent<EnemyAI>().setAlert(alertLevel + Time.deltaTime * alertRate);
        else if (alertLevel > 0) Guard.GetComponent<EnemyAI>().setAlert(alertLevel - Time.deltaTime * alertDropoffRate);

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.name == "Player") playerSeen = true;

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Player") playerSeen = false;

    }
    public static bool IsPointInsideCone(Vector3 point, Vector3 coneOrigin, Vector3 coneDirection, float maxAngle, float maxDistance)
    {
        var distanceToConeOrigin = (point - coneOrigin).magnitude;
        if (distanceToConeOrigin < maxDistance)
        {
            var pointDirection = point - coneOrigin;
            var angle = Vector3.Angle(coneDirection, pointDirection);
            if (angle < maxAngle)
                return true;
        }
        return false;
    }
}
