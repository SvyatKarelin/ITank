using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static Vector3 GetVelocity(Transform Obj) => CheckComponent<VelocityCalculator>(Obj).Velocity;
}