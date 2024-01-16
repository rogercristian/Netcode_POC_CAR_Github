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

    [SerializeField] float hp = 10f;// manter?
    [SerializeField] private Vector3 centerOfMass;


    private float currentBrakeForce;
    private Rigidbody rb;
   //PlayerMiscController playerMiscController;

    [SerializeField] private CinemachineVirtualCamera vc;
    //  [SerializeField] private AudioListener audioListener;

    InputManager inputManager;

    public override void OnNetworkSpawn()
    {

        if (!IsOwner)
        {
            vc.Priority = 0;
            //enabled = false;
            //Destroy(this);
        }
        else
        {
            vc.Priority = 1;
        }

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        inputManager = GetComponent<InputManager>();
    }
    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    rb.centerOfMass = centerOfMass;
    //    inputManager = GetComponent<InputManager>();
    //  //  playerMiscController = GetComponent<PlayerMiscController>();
    //}

   

    // finds the corresponding visual wheel
    // correctly applies the transform
    
    public void FixedUpdate()
    {       
        //Impede que o player controle quando capota
      //  if (!playerMiscController.isCarController) return;
       
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

  
}
