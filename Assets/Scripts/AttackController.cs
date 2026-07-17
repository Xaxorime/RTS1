using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;
    public Material idleStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && targetToAttack == null)
        {
            targetToAttack = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && targetToAttack != null)
        {
            targetToAttack = null;
        }
    }
    public void SetIdleStateMaterial()
    {
        GetComponent<Renderer>().material = idleStateMaterial;
    }
    public void SetFollowStateMaterial()
    {
        GetComponent<Renderer>().material = followStateMaterial;
    }
    public void SetAttackStateMaterial()
    {
        GetComponent<Renderer>().material = attackStateMaterial;
    }
    private void OnDrawGizmos()
    {
        //Follow Distance Area
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10f);

        //Attack Distance Area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);

        //Stop Distance Area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1.2f);

    }
}
