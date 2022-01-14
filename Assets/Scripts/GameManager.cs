using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager gm;
    public int lives = 4;
    public float health = 100;

    [HideInInspector]public int scores = 0, shurikenCount = 3, handgunAmmo = 8, assaultRifleAmmo = 20;

    private void Awake()
    {

        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
