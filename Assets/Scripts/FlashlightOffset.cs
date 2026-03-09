using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    private Vector3 Offset;
    public GameObject FollowCam;

    [SerializeField] private float MoveSpeed = 15f;

    public Light FlashLight;

    void Start()
    {
        Offset = transform.position - FollowCam.transform.position;
    }

    void Update()
    {
        transform.position = FollowCam.transform.position + Offset;

        transform.rotation = Quaternion.Slerp(transform.rotation, FollowCam.transform.rotation, MoveSpeed * Time.deltaTime);
    }
}
