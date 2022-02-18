using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingOrAttack : MonoBehaviour
{
    public GameObject[] shootingItem;
    public Transform shootingPoint;
    private bool canshoot = true;
    public static bool isAttack;
    public static int attackPoint = 2;

    private void Update()
    {
        if((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.L)) && !Player.isDead && !BossVehicle.isDead)
        {
            if (SwitchWeapons.shuriken && FindObjectOfType<GameManager>().shurikenCount > 0)
            {
                FindObjectOfType<GameManager>().shurikenCount--;
                GetComponent<Animator>().SetTrigger("ShurikenThrow");
                AudioManager.instance.PlaySFX("throwing");
                Shoot(0);
            }
            else if(SwitchWeapons.handgun && FindObjectOfType<GameManager>().handgunAmmo > 0)
            {
                FindObjectOfType<GameManager>().handgunAmmo--;
                GetComponent<Animator>().SetTrigger("ShootHG");
                AudioManager.instance.PlaySFX("shootHandgun");
                Shoot(1);
            }
            else if (SwitchWeapons.AR && FindObjectOfType<GameManager>().assaultRifleAmmo > 0)
            {
                FindObjectOfType<GameManager>().assaultRifleAmmo -= 3;
                if(FindObjectOfType<GameManager>().assaultRifleAmmo <= 0)
                {
                    FindObjectOfType<GameManager>().assaultRifleAmmo = 0;
                }
                GetComponent<Animator>().SetTrigger("ShootAR");
                AudioManager.instance.PlaySFX("shootAssaultRifle");
                Shoot(2);
            }
        }

        if((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.K)) && !Player.isDead && !Loot.playerInZone && !BossVehicle.isDead)
        {
            GetComponent<Animator>().SetTrigger("Attack");
            AudioManager.instance.PlaySFX("Attack");
            isAttack = true;
        }
    }

    void Shoot(int value)
    {
        if (!canshoot)
        {
            return;
        }

        GameObject si = Instantiate(shootingItem[value], shootingPoint);
        si.transform.parent = null;
    }

    public void ResetAttack()
    {
        isAttack = false;
    }
}
