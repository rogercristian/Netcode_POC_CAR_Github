using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteTransform : MonoBehaviour
{
    [SerializeField] Vector3 vector3;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = vector3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
