using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public static class Utilits
{
    public static void SetClip(AudioSource Source,AudioClip Clip, float Vol)
    {
        if (Source.clip == Clip) return;
        Source.Stop();
        Source.clip = Clip;
        Source.volume = Vol;
        Source.Play();
    }

    public static T CheckComponent<T>(Transform Transform) where T : Component
    {
        T Component = Transform.GetComponent<T>();
        if(!Component) Component = Transform.AddComponent<T>();
        return Component;
    }

    public static Bounds GetBoundingBox(Transform Obj)
    {
        Renderer r = Obj.GetComponent<Renderer>();
        if (r == null) { var EmptyBounds = new Bounds(Obj.position, Vector3.zero); return EmptyBounds; }
        Bounds bounds = r.bounds;
        return bounds;
    }

    public static Bounds GetHierarchyBounds(Transform transform)
    {
        Bounds SelfBB = GetBoundingBox(transform);
        Vector3 extents = SelfBB.extents;

        for (int ChildInd = 0; ChildInd < transform.childCount; ChildInd++)
        {
            Bounds ChildBB = GetBoundingBox(transform.GetChild(ChildInd));
            Vector3 ExtensFromCenter = ChildBB.extents + (ChildBB.center - SelfBB.center);
            extents = Vector3.Max(extents, ExtensFromCenter);
        }
        return new Bounds(SelfBB.center, extents * 2);
    }

    public static bool CompareWithError(Vector3 Vec1, Vector3 Vec2, float Error) => (Vec1-Vec2).magnitude < Error;

    public static List<Transform> GetObjectsInHierarchy(Transform Parent)
    {
        List <Transform> Result = new List <Transform>();
        for (int ChildInd = 0; ChildInd < Parent.childCount; ChildInd++)
        {
            Transform Child = Parent.GetChild(ChildInd);
            if (Child.childCount > 0) Result.AddRange(GetObjectsInHierarchy(Child));
            Result.Add(Child);
        }
        return Result;
    }

    private static List<Transform> GetChildsWithTag(Transform Obj, string Tag)
    {
        List<Transform> Res = new List<Transform>();
        Debug.Log(Obj.tag);
        if (Obj.tag == Tag) Res.Add(Obj.transform);
        for (int ChildInd = 0; ChildInd < Obj.childCount; ChildInd++) Res.AddRange(GetChildsWithTag(Obj.GetChild(ChildInd), Tag));
        return Res;
    }

    public static List<Transform> GetAllWithTag(string Tag)
    {
        List <Transform> Res = new List <Transform>();
        foreach (GameObject Obj in Resources.FindObjectsOfTypeAll(typeof(GameObject))) Res.AddRange(GetChildsWithTag(Obj.transform, Tag));
        return Res;
    }

    public static Vector2 GetRandomOnCircle()
    {
        float RndAng = Random.Range(0, 2 * Mathf.PI);
        return new Vector2(Mathf.Cos(RndAng), Mathf.Sin(RndAng));
    }

    public static Vector3 GetGround(Vector3 Pos)
    {
        if (Physics.Raycast(Pos, Vector3.down, out RaycastHit hit)) return hit.point;
        else return Pos;
    }

    public static Vector3 GetVelocity(Transform Obj) => CheckComponent<VelocityCalculator>(Obj).Velocity;
}