using UnityEngine;
using UnityEngine.AI;

public class DoorNavMesh : MonoBehaviour
{
    public NavMeshObstacle obstacle; 
    public bool isOpen = false;

    public void OpenDoor()
    {
        isOpen = true;
        obstacle.enabled = false; 
    }

    public void CloseDoor()
    {
        isOpen = false;
        obstacle.enabled = true; 
    }
}