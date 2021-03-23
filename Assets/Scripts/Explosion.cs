using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    AudioSource _explosion;
    void Start()
    {
        _explosion = GetComponent<AudioSource>();
        _explosion.Play();
    }
}
