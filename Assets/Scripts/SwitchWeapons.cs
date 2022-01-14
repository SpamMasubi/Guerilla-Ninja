using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWeapons : MonoBehaviour
{
    public Image[] playerWeapons;

    public static bool shuriken = true, handgun, AR;

    private float rgb = 0.3301887f; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //if alpha1, shuriken available
        {
            AudioManager.instance.PlaySFX("weaponSwitch");
            shuriken = true;
            playerWeapons[0].color = new Color(1, 1, 1);

            AR = false;
            handgun = false;
            playerWeapons[1].color = new Color(rgb, rgb, rgb);
            playerWeapons[2].color = new Color(rgb, rgb, rgb);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AudioManager.instance.PlaySFX("weaponSwitch");
            playerWeapons[1].color = new Color(1, 1, 1);
            handgun = true;

            shuriken = false;
            AR = false;
            playerWeapons[0].color = new Color(rgb, rgb, rgb);
            playerWeapons[2].color = new Color(rgb, rgb, rgb);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AudioManager.instance.PlaySFX("weaponSwitch");
            AR = true;
            playerWeapons[2].color = new Color(1, 1, 1);

            handgun = false;
            shuriken = false;
            playerWeapons[1].color = new Color(rgb, rgb, rgb);
            playerWeapons[0].color = new Color(rgb, rgb, rgb);

        }
    }
}
