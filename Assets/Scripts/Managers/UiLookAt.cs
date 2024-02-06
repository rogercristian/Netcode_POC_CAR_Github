using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class UiLookAt : MonoBehaviour
{
    private void LateUpdate()
    {
      
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward);
    }
}
