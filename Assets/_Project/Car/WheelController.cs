using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private Transform wheelModel;
    public bool steerable;
    public bool motorized;
    [HideInInspector] public WheelCollider wheelCollider;
    private Vector3 _position;
    private Quaternion _rotation;
    
    private void Awake()
    {
        wheelCollider = GetComponent<WheelCollider>();
    }
    
    private void Update()
    {
        wheelCollider.GetWorldPose(out _position, out _rotation);
        wheelModel.transform.position = _position;
        wheelModel.transform.rotation = _rotation;
    }
}