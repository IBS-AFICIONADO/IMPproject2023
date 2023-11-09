using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : MonoBehaviour
{
    // Start is called before the first frame update
    private int sceneId;
    private int nextSceneID;
    [SerializeField]
    public Scene GO;
    public playerMovement Player;
    

    // Start is called before the first frame update
    private void Start()
    {
        nextSceneID = SceneManager.GetActiveScene().buildIndex + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.touch)
        {

            SceneManager.LoadScene("GameOver");
        }
    }


}
