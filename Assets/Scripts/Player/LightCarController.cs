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
    void Update()
    {
        if (!IsOwner) { return; }

        if (inputManager.GetHardBrakePressed())
        {
            EmissiveIntensity(isAtive);
        }
        else
        {
            EmissiveIntensity(!isAtive);
        }

        HighBeam();
    }
    void EmissiveIntensity(bool isAtive)
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

    private void HighBeam()
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
}
