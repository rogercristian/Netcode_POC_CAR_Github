using UnityEngine;

public class HeliBehaviour : MonoBehaviour
{
    [SerializeField]private bool rotorOn = false;
    [SerializeField] private float rotateSpeed = 15f;
    [SerializeField] private GameObject helicePrincipal, heliceTrazeira;
    void Update()
    {
        if (rotorOn)
        {
            helicePrincipal.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.World);    
            heliceTrazeira.transform.Rotate(rotateSpeed * Time.deltaTime,0,0, Space.World);
        }
            
    }
}
