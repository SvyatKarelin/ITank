using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
}