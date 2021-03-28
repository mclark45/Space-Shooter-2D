using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speedOfPowerUp = 3.0f;
    [SerializeField]  //0 = Triple Shot 1 = Speed 2 = Debuff 3 Shield 4 = Health 5 = Homing Missile 6 = Ammo
    private int _powerUpID;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speedOfPowerUp * Time.deltaTime);
        if (Input.GetKey(KeyCode.C))
        {
            if ((transform.position - _player.transform.position).magnitude < 100)
            {
                Vector3 distance = _player.transform.position - transform.position;
                transform.Translate(distance * _speedOfPowerUp * Time.deltaTime);
            }
        }

        if (transform.position.y < -5.4f)
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
                switch (_powerUpID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedPowerUpActive();
                        break;
                    case 2:
                        player.DebuffActive();
                        break;
                    case 3:
                        player.ShieldPowerUpActive();
                        break;
                    case 4:
                        player.CollectedHealthPowerup();
                        break;
                    case 5:
                        player.HomingMissleActive();
                        break;
                    case 6:
                        player.CollectedAmmoPowerUp();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
   
            Destroy(this.gameObject);
        }
    }
}
