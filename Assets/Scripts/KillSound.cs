using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSound : MonoBehaviour
{
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }

}
