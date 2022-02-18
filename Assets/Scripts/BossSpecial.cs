using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpecial : MonoBehaviour
{
    public Transform specialGun;
    public GameObject specialBullet;
    float attackTimer = 0f;
    public float timeOfAttack;

    // Start is called before the first frame update
    void Start()
    {
        specialGun = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (BossStart.startBoss && !BossVehicle.isDead)
        {
            if (attackTimer <= 0f)
            {
                attackTimer = timeOfAttack;
            }
            else
            {
                if (attackTimer > 0f)
                {
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0f)
                    {
                        Instantiate(specialBullet, specialGun.position, specialGun.rotation);
                    }
                }
            }
        }
    }
}
