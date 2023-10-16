using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerups : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private float invisDeplete;
    [SerializeField]
    private float invisCharge;
    public float invisibleTimer = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && invisibleTimer > 0)
        {
            invisible();
        }
        else
        {
            invisibleTimer += invisCharge * Time.deltaTime;
            playerObject.tag = "Player";
        }

    }

    private void invisible()
    {
        invisibleTimer -= invisDeplete * Time.deltaTime;
        playerObject.tag = "Invisible";  
    }

   
}
