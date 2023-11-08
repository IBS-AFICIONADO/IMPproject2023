using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource EMP;
    // Start is called before the first frame update
    void Start()
    {
        EMP.Play();
    }

}
