using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CinemashineShake : NetworkBehaviour
{
    //  public static CinemashineShake Instance { get; private set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float shakeTimeTotal;
    private float startIntensity;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

    }
    //private void Awake()
    //{
    // //   Instance = this;
    //    cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    //}

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        startIntensity = intensity;
        shakeTimeTotal = time;
        shakeTimer = time;
    }
    void Update()
    {
        if (!IsOwner) return;

        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
           cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                    Mathf.Lerp(startIntensity, 0, 1 - (shakeTimer / shakeTimeTotal));
            }
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void AttackServerRpc(float intensity, float time)
    {
        
        AttackClientRpc(intensity, time);
    }

    [ClientRpc]
    private void AttackClientRpc(float intensity, float time)
    {
        ShakeCamera(intensity, time);
    }
}
