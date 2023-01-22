using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueClick : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] float torqueValue = 5.0f;
    private void Awake() 
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 torque = new Vector3(0,torqueValue,0);
            _rb.AddTorque(torque);
        }
    }
}
