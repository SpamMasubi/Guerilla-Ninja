using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage;
    public float speed = 10;
    [HideInInspector]
    public int direction = 1;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (direction == 1 && !BossStart.startBoss)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        if (BossStart.startBoss)
        {
            rb2d.velocity = new Vector3(-speed, 0, 0);
        }
        else
        {
            rb2d.velocity = new Vector3(speed * direction, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            return;
        }
        //Destroy
        if (collision.tag == "Player")
        {
            if (!Player.isInvincible && !ShootingOrAttack.isAttack)
            {
                FindObjectOfType<Healthbar>().LoseHealth(damage);
            }
            Destroy(gameObject);
        }
    }
}
