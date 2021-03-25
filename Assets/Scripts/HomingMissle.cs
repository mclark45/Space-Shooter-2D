using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    public float speed;
    public float rotateSpeed;

    [SerializeField]
    private GameObject _explosion;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Enemy").transform;

        if (target == null)
        {
            Debug.LogError("Enemy is Null");
        }

        if (rb == null)
        {
            Debug.LogError("rigid body is Null");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
        {
            rb.velocity = transform.up * speed;
        }
        else
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * speed;
            Destroy(this.gameObject, 5.0f);
        }
    }

    void OnTriggerEnter2D()
    {
        GameObject explosionEffect = Instantiate(_explosion, transform.position, transform.rotation);
        Destroy(explosionEffect, 2.4f);
        Destroy(this.gameObject);
    }

}