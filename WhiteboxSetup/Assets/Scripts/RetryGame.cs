using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryGame : MonoBehaviour
{
    public AudioSource defeated;
    public void LoadGame()
    {
        SceneManager.LoadScene("Terrain Trial");
    }
    private void Start()
    {
        defeated.Play();
    }
}
