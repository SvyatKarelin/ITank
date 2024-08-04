using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArtilleryCannon : Cannon
{
    protected override void ShootShell(GameObject Shell)
    {
        CannonAudioSource.Play();
        Shell = Instantiate(ShellPref, CannonTransform.position, CannonTransform.rotation);

        Shell.GetComponent<Shell>().Ignore.Add(gameObject);
        Shell.GetComponent<Rigidbody>().velocity = CannonTransform.forward * CannonStrength;
    }

    Vector2 CalculateAngles( Vector3 Target )
    {
        float HorizontalAng = Quaternion.LookRotation(Target - transform.position).eulerAngles.y;
        float Distance = Vector3.Distance(CannonTransform.position, Target);
        float YProjection = Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(Distance, 4) * (Mathf.Pow(CannonStrength, 4) - Mathf.Pow(9.81f, 2) * Mathf.Pow(Distance, 2))) / Mathf.Pow(Distance, 2) + Mathf.Pow(CannonStrength, 2)) / Mathf.Sqrt(2);
        float VerticalAng = Mathf.Asin(YProjection / CannonStrength) * Mathf.Rad2Deg;
        return new Vector2(VerticalAng, HorizontalAng);
    }

    private new void Update()
    {
        if (CannonTarget is not null)
        {
            Vector2 Angles;
            Vector3 PrevPos = CannonTarget.GetPos(), Proactive = CannonTarget.GetPos();

            //do
            //{
                //PrevPos = Proactive;
            Angles = CalculateAngles(PrevPos);
                //Proactive = PrevPos + Utilits.GetVelocity(CannonTarget.GetTargetTransform()) * (2*CannonStrength*Mathf.Sin(Angles.x))/9.81f;
                //print(Utilits.GetVelocity(CannonTarget.GetTargetTransform()));
            //} while ((Proactive - PrevPos).magnitude > 100);

            Quaternion CannonRotation = new Quaternion();
            CannonRotation.eulerAngles = new Vector3(-Angles.x, Angles.y, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, CannonRotation, RotationSpeed * Time.deltaTime);

            if (Utilits.CompareWithError(transform.rotation.eulerAngles, CannonRotation.eulerAngles, 2f)) IsAimed = true;
            else IsAimed = false;
        }
    }
}
