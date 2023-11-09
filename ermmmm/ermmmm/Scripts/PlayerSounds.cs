using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] AudioSource SFXSource;

    public AudioClip Defeated;
    public AudioClip ThrowGrenade;
    public AudioClip EMPgrenade;

    public void Start()
    {
        SFXSource = GetComponent<AudioSource>();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
