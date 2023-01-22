using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spin : MonoBehaviour
{
    public float spinMagnitude;
    // Start is called before the first frame update
    public void InteractWithSpin(Spin otherSpin)
    {
        Rigidbody rb = otherSpin.GetComponent<Rigidbody>();
        
        Vector3 relativePosition = otherSpin.transform.position - transform.position;
        float distance = relativePosition.magnitude;
        relativePosition = Vector3.Normalize(relativePosition);

        Vector3 torque = Vector3.Cross(otherSpin.transform.up, transform.up);
        torque += - 3 * Vector3.Dot(transform.up, relativePosition) * Vector3.Cross(otherSpin.transform.up, relativePosition);
        torque = spinMagnitude * otherSpin.spinMagnitude / (4.0f * 3.1414f * distance * distance * distance) * torque;

        rb.AddTorque(torque);
    }
}
