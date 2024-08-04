using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : DestructableVehicle
{
    [SerializeField] private Cannon TankCannon;
    [SerializeField] private Transform CameraTransform;
    [SerializeField] private Transform CameraAnchor;
    [SerializeField] private float MouseSensetivity = 5;
    [SerializeField] private float CameraRange = 10;
    [SerializeField] private float MaxSpeed = 10;
    [SerializeField] private float RotationSpeed = 1;
    [SerializeField] private float Acceleration = 35;
    [SerializeField] private float Deceleration = 35;
    [SerializeField] private AudioClip Idle;
    [SerializeField] private AudioClip Driving;
    private AudioSource audioSourse;
    private Rigidbody Rigitbody;
    private Vector2 Rot;
    public override void Start()
    {
        HealthPoints = StartHealth;
        Rigitbody = Utilits.CheckComponent<Rigidbody>(transform);
        audioSourse = Utilits.CheckComponent<AudioSource>(transform);
        audioSourse.loop = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    Vector3 CalcCameraPos(Vector3 RadRot, float Range) => 
        CameraAnchor.position + new Vector3(Mathf.Cos(RadRot.x) * Range * Mathf.Cos(RadRot.y), Range * Mathf.Sin(RadRot.y), Mathf.Sin(RadRot.x) * Range * Mathf.Cos(RadRot.y));
    Vector3 GetShootPos()
    {
        Ray ray = CameraTransform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.point;
        else return ray.origin + ray.direction*100;
    }

    void Update()
    {
        Rot += new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")) * MouseSensetivity;
        Rot = new Vector2(Rot.x, Mathf.Clamp(Rot.y, -10, 80));

        if (Rigitbody.velocity.magnitude <= MaxSpeed) { 
            float ForwardAcc = Acceleration * Time.deltaTime * Input.GetAxis("Vertical");
            Rigitbody.velocity += new Vector3(transform.forward.x * ForwardAcc, 0f, transform.forward.z * ForwardAcc); 
        }
        //Braking
        if (Input.GetAxis("Vertical") == 0) {
            Utilits.SetClip(audioSourse ,Idle, 0.1f);
            if (Rigitbody.velocity.magnitude >= 1) Rigitbody.velocity += transform.forward * (Deceleration * Time.deltaTime * Vector3.Dot(transform.forward, Rigitbody.velocity) < 0 ? 1 : -1);
        } else {
            Utilits.SetClip(audioSourse, Driving, 0.4f);
        }

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

        TankCannon.LookAt(GetShootPos());
        if (Input.GetMouseButtonDown(0)) TankCannon.Shoot();
    }
}
