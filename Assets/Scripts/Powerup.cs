using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speedOfPowerUp = 3.0f;

    void Update()
    {
        transform.Translate(Vector3.down * _speedOfPowerUp * Time.deltaTime);

        if (transform.position.y < -5.4)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.TripleShotActive();
            }
   
            Destroy(this.gameObject);
        }
    }
}
