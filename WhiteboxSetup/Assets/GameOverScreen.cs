using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public void RestartButton()
    {
        SceneManager.LoadScene("TerrainTrial");
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
