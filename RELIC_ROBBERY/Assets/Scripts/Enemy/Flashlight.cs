using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Transform coneOrigin;
    public Transform Player;
    public Transform Guard;
    float alertRate;
    float alertDropoffRate;
    // Start is called before the first frame update
    void Start()
    {
        alertRate = Guard.GetComponent<EnemyAI>().alertRate;
        alertDropoffRate = Guard.GetComponent<EnemyAI>().alertDropoffRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPointInsideCone(Player.transform.position, coneOrigin.transform.position, coneOrigin.transform.forward, 16.11f, 10.89f))
        {
            Guard.GetComponent<EnemyAI>().alertLevel += Time.deltaTime * alertRate;
        }
        else if (Guard.GetComponent<EnemyAI>().alertLevel > 0) Guard.GetComponent<EnemyAI>().alertLevel -= Time.deltaTime * alertDropoffRate;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player") Debug.Log("player detected");
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
