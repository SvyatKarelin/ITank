using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArtilleryCannon : Cannon
{
    [SerializeField] private float VerticalAngle;

    protected override void ShootShell(GameObject Shell)
    {
        CannonAudioSource.Play();
        Shell = Instantiate(ShellPref, CannonTransform.position, CannonTransform.rotation);
        CalculateBallistics(CannonTarget.GetPos(),out float Vel ,out Vector2 Ang);

        Shell.GetComponent<Shell>().Ignore.Add(gameObject);
        Shell.GetComponent<Rigidbody>().velocity = CannonTransform.forward * Vel;
    }

    void CalculateBallistics( Vector3 Target, out float Vel, out Vector2 Angles)
    {
        Angles = new Vector2(45, Quaternion.LookRotation(Target - transform.position).eulerAngles.y);
        float Distance = Vector3.Distance(CannonTransform.position, new Vector3(Target.x, CannonTransform.position.y , Target.z));
        float HeightDelta = Target.y - CannonTransform.position.y;
        Vel = Mathf.Sqrt((-Physics.gravity.y*Mathf.Pow(Distance, 2)) / (Mathf.Cos(Angles.x*Mathf.Deg2Rad)*Mathf.Cos(Angles.x * Mathf.Deg2Rad)*Distance - Mathf.Pow(Mathf.Cos(Angles.x * Mathf.Deg2Rad), 2)*HeightDelta))/ Mathf.Sqrt(2);
        print(Vel);
    }

    private new void Update()
    {
        if (CannonTarget is not null)
        {
            //Vector2 Angles;
            //Vector3 PrevPos = CannonTarget.GetPos(), Proactive = CannonTarget.GetPos();

            /*do
            {
                PrevPos = Proactive;
                Angles = CalculateAngles(PrevPos);
                Proactive = PrevPos + Utilits.GetVelocity(CannonTarget.GetTargetTransform()) * (2*CannonStrength*Mathf.Sin(Angles.x))/9.81f;
                print(Utilits.GetVelocity(CannonTarget.GetTargetTransform()));
            } while ((Proactive - PrevPos).magnitude > 100);*/

            CalculateBallistics(CannonTarget.GetPos(), out float Vel, out Vector2 Ang);
            Quaternion CannonRotation = new Quaternion();
            CannonRotation.eulerAngles = new Vector3(-Ang.x, Ang.y, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, CannonRotation, RotationSpeed * Time.deltaTime);
            print(Vel);

            if (Utilits.CompareWithError(transform.rotation.eulerAngles, CannonRotation.eulerAngles, 2f)) IsAimed = true;
            else IsAimed = false;
        }
    }
}
