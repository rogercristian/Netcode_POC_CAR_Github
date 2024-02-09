using Cinemachine;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}
public class CarController : NetworkBehaviour
{
    [SerializeField] List<AxleInfo> axleInfos; // the information about each individual axle
    [SerializeField] float maxMotorTorque; // maximum torque the motor can apply to wheel
    [SerializeField] float maxSteeringAngle; // maximum steer angle the wheel can have
    [SerializeField] float maxBrakeForce;

    [SerializeField] float maxVelocity = 60f;
    float maxSpeed;
    [SerializeField] float hp = 10f;// manter?
                                    //  [SerializeField] private Vector3 centerOfMass;

    private TakeDamage takeDamage;
    private float currentBrakeForce;
    private Rigidbody rb;
    //PlayerMiscController playerMiscController;

    [SerializeField] private CinemachineVirtualCamera vc;
    //  [SerializeField] private AudioListener audioListener;

    InputManager inputManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        takeDamage = GetComponent<TakeDamage>();

    }
    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            vc = FindAnyObjectByType<CinemachineVirtualCamera>();
            vc.Follow = gameObject.transform;
            vc.LookAt = gameObject.transform;
            vc.Priority = 1;
            //   rb = GetComponent<Rigidbody>();
            // rb.centerOfMass = centerOfMass;
            //  inputManager = GetComponent<InputManager>();

            // vc.Priority = 0;
        }
        //else
        //{
        //  //  vc.Priority = 0;
        //}

    }
    public void FixedUpdate()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }
        //Impede que o player controle quando capota
        //  if (!takeDamage.isCarController) return;
        //  if (!GameManager.Instance.IsGamePlaying()) return;

        maxSpeed = rb.velocity.magnitude;
        maxSpeed = Mathf.Clamp(maxSpeed, 0f, maxVelocity);

        rb.velocity = rb.velocity.normalized * maxSpeed;

        //Debug.Log(Mathf.Round(maxSpeed));

        Vector2 pos = inputManager.GetMoveDirection();
        Vector2 acceleration = inputManager.GetAcceleratePressed();
        float motor = maxMotorTorque * hp * acceleration.y;
        float steering = maxSteeringAngle * pos.x;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;

            }
            if (inputManager.GetHardBrakePressed())
            {
                currentBrakeForce = maxBrakeForce * hp;
            }
            else
            {
                currentBrakeForce = 0;
            }

            axleInfo.leftWheel.brakeTorque = currentBrakeForce;
            axleInfo.rightWheel.brakeTorque = currentBrakeForce;

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        collider.GetWorldPose(out Vector3 position, out Quaternion rotation);

        visualWheel.transform.SetPositionAndRotation(position, rotation);
    }

    //public float GetMaxVelocity()
    //{
    //    return maxVelocity ;
    //}

    public float Velocimeter()
    {
        return maxSpeed = Mathf.Round(Mathf.Clamp(maxSpeed, 0f, maxVelocity));

    }

    public float MaxSpeed()
    {
        return maxVelocity;
    }
}
