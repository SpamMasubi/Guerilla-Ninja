using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWeapons : MonoBehaviour
{
    public Image[] playerWeapons;

    public static bool shuriken = true, handgun, AR;

    private float rgb = 0.3301887f;

    private void Start()
    {
        if (shuriken)
        {
            shurikenSelect();
        }
        else if (handgun)
        {
            handGunSelect();
        }
        else
        {
            assaultRifleSelected();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPause && !BossVehicle.isDead && !FinalBoss.isDead)
        {
            //Controller
            if (Input.GetButtonDown("Debug Next"))
            {
                AudioManager.instance.PlaySFX("weaponSwitch");
                if (shuriken)
                {
                    handGunSelect();
                }
                else if (handgun)
                {
                    assaultRifleSelected();
                }
                else
                {
                    shurikenSelect();
                }
            }
            else if (Input.GetButtonDown("Debug Previous"))
            {
                AudioManager.instance.PlaySFX("weaponSwitch");
                if (AR)
                {
                    handGunSelect();
                }
                else if (handgun)
                {
                    shurikenSelect();
                }
                else
                {
                    assaultRifleSelected();
                }
            }

            //Keyboards
            if (Input.GetKeyDown(KeyCode.Alpha1)) //if alpha1, shuriken available
            {
                AudioManager.instance.PlaySFX("weaponSwitch");
                shurikenSelect();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AudioManager.instance.PlaySFX("weaponSwitch");
                handGunSelect();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AudioManager.instance.PlaySFX("weaponSwitch");
                assaultRifleSelected();
            }
        }
    }

    void shurikenSelect()
    {
        shuriken = true;
        playerWeapons[0].color = new Color(1, 1, 1);

        AR = false;
        handgun = false;
        playerWeapons[1].color = new Color(rgb, rgb, rgb);
        playerWeapons[2].color = new Color(rgb, rgb, rgb);
    }

    void handGunSelect()
    {
        playerWeapons[1].color = new Color(1, 1, 1);
        handgun = true;

        shuriken = false;
        AR = false;
        playerWeapons[0].color = new Color(rgb, rgb, rgb);
        playerWeapons[2].color = new Color(rgb, rgb, rgb);
    }

    void assaultRifleSelected()
    {
        AR = true;
        playerWeapons[2].color = new Color(1, 1, 1);

        handgun = false;
        shuriken = false;
        playerWeapons[1].color = new Color(rgb, rgb, rgb);
        playerWeapons[0].color = new Color(rgb, rgb, rgb);
    }
}
