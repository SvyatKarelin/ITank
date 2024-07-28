using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform CameraTransform;
    public Transform CameraAnchor;
    public float MouseSensetivity = 5;
    public float CameraRange = 10;
    public float MaxSpeed = 10;
    public float RotationSpeed = 1;
    public float Acceleration = 35;
    public float Deceleration = 35;
    private Rigidbody Rigitbody;
    private Vector2 Rot;
    void Start()
    {
        Rigitbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    Vector3 CalcCameraPos(Vector3 RadRot, float Range) => 
        CameraAnchor.position + new Vector3(Mathf.Cos(RadRot.x) * Range * Mathf.Cos(RadRot.y), Range * Mathf.Sin(RadRot.y), Mathf.Sin(RadRot.x) * Range * Mathf.Cos(RadRot.y));
    void Update()
    {
        Rot += new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * MouseSensetivity;
        Rot = new Vector2(Rot.x, Mathf.Clamp(Rot.y, -10, 80));

        if (Rigitbody.velocity.magnitude <= MaxSpeed) { 
            float ForwardAcc = Acceleration * Time.deltaTime * Input.GetAxis("Vertical");
            Rigitbody.velocity += new Vector3(transform.forward.x * ForwardAcc, 0f, transform.forward.z * ForwardAcc); 
        }
        //Braking
        if (Input.GetAxis("Vertical") == 0 && Rigitbody.velocity.magnitude >= 1) Rigitbody.velocity += transform.forward * (Deceleration * Time.deltaTime * Vector3.Dot(transform.forward, Rigitbody.velocity) < 0 ? 1 : -1);

        Vector3 AngVel = Rigitbody.angularVelocity;
        AngVel.y = Input.GetAxis("Horizontal") * RotationSpeed;
        Rigitbody.angularVelocity = AngVel;

        Vector2 RadRot = Rot * Mathf.Deg2Rad;
        transform.GetComponent<Rigidbody>();
        Vector3 CameraNewPos = CalcCameraPos(RadRot, CameraRange);
        RaycastHit hit;
        if (Physics.Raycast(CameraAnchor.position, CameraNewPos - CameraAnchor.position, out hit, CameraRange + 2)) CameraNewPos = CalcCameraPos(RadRot, hit.distance - 2);
        CameraTransform.position = CameraNewPos;
        CameraTransform.LookAt(CameraAnchor.position);
    }
}
