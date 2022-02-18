using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingMissile : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float rotateSpeed = 200f;
    public int damage;
    public GameObject explosionEffect;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, -transform.right).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = -transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !Player.isInvincible && !ShootingOrAttack.isAttack)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            FindObjectOfType<Healthbar>().LoseHealth(damage);
            Destroy(gameObject);
        }

        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
