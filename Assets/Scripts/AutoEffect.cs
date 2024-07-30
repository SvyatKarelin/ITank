using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEffect : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    void Start()
    {
        _particleSystem = transform.GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        if(!_audioSource) _audioSource.Play();
        _particleSystem.Play();
    }
    private void Update()
    {
        if(!_particleSystem.IsAlive()) Destroy(gameObject);
    } 
}
