using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private float _despawnLaser = 8.0f;
    
    void Update()
    {
        LaserControl();
    }

    void LaserControl()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= _despawnLaser)
        {
            Destroy(this.gameObject);
        }
    }
}
