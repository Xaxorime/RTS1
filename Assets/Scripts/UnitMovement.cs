using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public class UnitMovement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;
    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground)) 
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
