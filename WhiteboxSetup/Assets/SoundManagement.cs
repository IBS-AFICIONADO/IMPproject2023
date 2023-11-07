using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManagement : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource BackgroundMusic;
    [SerializeField] AudioSource SFX;

    [Header("Audio Clips")]
    public AudioClip Background;
    public AudioClip RobotSFX;
    public AudioClip RobotDie;
    public AudioClip PlayerLeftLeg;
    public AudioClip PlayerRightLeg;

    private void Start()
    {
        BackgroundMusic.clip = Background;
        BackgroundMusic.Play();
    }

}

