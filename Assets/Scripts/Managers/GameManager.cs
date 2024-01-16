using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isNight = false;

    public new Light light;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Tem outro GameManager aqui! =O ");
        }
        Instance = this;       
    }
    private void Update()
    {
        if (isNight)
        {
            light.GetComponent<Light>().intensity = 10.0f;
        }
        else
        {
            light.GetComponent<Light>().intensity = 100000.0f;

        }
    }
}
