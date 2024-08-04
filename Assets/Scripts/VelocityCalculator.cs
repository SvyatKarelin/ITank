using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCalculator : MonoBehaviour
{
    [HideInInspector] public Vector3 Velocity;
    private Vector3 PrevPos;

    private void Start()
    {
        PrevPos = transform.position;
    }
    void Update()
    {
        Velocity = (transform.position - PrevPos)/Time.deltaTime;
        PrevPos = transform.position;
        print(Velocity);
    }
}
