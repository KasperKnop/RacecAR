using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private InputAction _moveAction;
    private bool _isAccelerating, _isReversing, _isTurningLeft, _isTurningRight;
    private float _driveSpeed, _steerSpeed;
    private Rigidbody _rb;
    private WheelController[] _wheels;
    
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;

    private void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _rb = GetComponent<Rigidbody>();
        _rb.centerOfMass += Vector3.up * centreOfGravityOffset;
        _wheels = GetComponentsInChildren<WheelController>();
    }
    
    private void Update()
    {
        var hInput = _moveAction.ReadValue<Vector2>().x;
        var vInput = _moveAction.ReadValue<Vector2>().y;
        
        var forwardSpeed = Vector3.Dot(transform.forward, _rb.linearVelocity);
        var speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);
        var currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        var currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);
        var isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);
        
        foreach (var wheel in _wheels)
        {
            if (wheel.steerable)
            {
                wheel.wheelCollider.steerAngle = hInput * currentSteerRange;
            }
            
            if (isAccelerating)
            {
                if (wheel.motorized)
                {
                    wheel.wheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.wheelCollider.brakeTorque = 0;
            }
            
            else
            {
                wheel.wheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.wheelCollider.motorTorque = 0;
            }
        }
    }
}
