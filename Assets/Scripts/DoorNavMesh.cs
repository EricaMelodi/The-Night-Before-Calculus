using UnityEngine;
using UnityEngine.AI;

public class DoorNavMesh : MonoBehaviour
{
    public NavMeshObstacle obstacle; // assign your door's obstacle
    public bool isOpen = false;

    public void OpenDoor()
    {
        isOpen = true;
        obstacle.enabled = false; // remove obstacle so monsters can pass
        // animate door opening here
    }

    public void CloseDoor()
    {
        isOpen = false;
        obstacle.enabled = true; // block again
        // animate door closing here
    }
}