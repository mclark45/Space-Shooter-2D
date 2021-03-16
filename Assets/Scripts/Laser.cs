using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _destroyLaser;
    
    void Update()
    {
        LaserControl();
    }

    void LaserControl()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= _destroyLaser)
        {
            Destroy(this.gameObject);
        }
    }
}
