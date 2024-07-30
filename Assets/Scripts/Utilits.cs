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
}