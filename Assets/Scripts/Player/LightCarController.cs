using Unity.Netcode;
using UnityEngine;

public class LightCarController : NetworkBehaviour
{
    [SerializeField] private GameObject m_BrakeObject;
    [SerializeField] private Light m_HightLightObject;
    [SerializeField] private float emissionForce;

    private bool isAtive = false;
    InputManager inputManager;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
    }

    //public override void OnNetworkDespawn()
    //{
    //    inputManager = GetComponent<InputManager>();

    //}
    void Update()
    {
        if (!IsOwner) { return; }

        Vector2 dir = inputManager.GetAcceleratePressed();

        if (inputManager.GetHardBrakePressed() || dir.y < 0)
        {
            EmissiveIntensityServerRpc(isAtive);
        }
        else
        {
            EmissiveIntensityServerRpc(!isAtive);
        }

        HighBeamServerRpc();
    }

    [ClientRpc]
    private void EmissiveIntensityClientRpc(bool isAtive)
    {
        isAtive = !isAtive;
        float emissiveIntensity;
        Color emissiveColor = Color.red;
        if (isAtive)
        {
            emissiveIntensity = emissionForce;
            m_BrakeObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", emissiveColor * emissiveIntensity);

        }
        else
        {
            emissiveIntensity = 1;

            m_BrakeObject.GetComponent<Renderer>().material.SetColor("_EmissiveColor", emissiveColor * emissiveIntensity);

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void EmissiveIntensityServerRpc(bool isAtive)
    {
        EmissiveIntensityClientRpc(isAtive);
    }

    [ClientRpc]
    private void HighBeamClientRpc()
    {
        if (GameManager.Instance.isNight)
        {

            m_HightLightObject.intensity = 40000f;

        }
        else
        {
            m_HightLightObject.intensity = 12.5f;

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void HighBeamServerRpc()
    {
        HighBeamClientRpc();
    }
}
