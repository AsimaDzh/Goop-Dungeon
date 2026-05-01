using UnityEngine;

enum NPCState
{
    Idle = 0,
    Moving = 1, 
    Interacting = 2,
    Inspecting = 3,
    Accepting = 4,
    Rejecting = 5
}


public class NPCBase : MonoBehaviour
{
    // Variables

    private void Update()
    {
        // switch case
        // state idle
        // state moving
        // state interacting
        // state Inspecting
        // state Accepting
        // state Rejecting
    }


    private void HandleIdle()
    {
    }


    private void HandleMoving()
    {
    }


    private void GrabObject()
    {
    }


    private void InspectiongObject()
    {

    }


    private void AcceptingObject()
    {
    }


    private void RejectingObject()
    {
    }
}
