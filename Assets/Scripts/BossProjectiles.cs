using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectiles : MonoBehaviour
{
    public float moveSpeed;

    Rigidbody2D rb;

    Player player;

    Vector2 moveDirection;

    public int damage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindObjectOfType<Player>();
        moveDirection = (player.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !Player.isInvincible && !ShootingOrAttack.isAttack)
        {
            FindObjectOfType<Healthbar>().LoseHealth(damage);
            Destroy(gameObject);
        }
    }

}
