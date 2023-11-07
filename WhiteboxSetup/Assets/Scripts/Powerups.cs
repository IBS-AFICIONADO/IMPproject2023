using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private Material[] materials = new Material[2];
    public magicTank magicTank;
    // Start is called before the first frame update
    void Start()
    {
        playerBody.GetComponent<MeshRenderer>().material = materials[0];

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && magicTank.magicMeter > 1 && !magicTank.cooldown)
        {
            invisible();
            magicTank.deplete = true;
        }
        else
        {
            magicTank.deplete = false;
            int visLayer = LayerMask.NameToLayer("target");
            playerObject.layer = visLayer;
            playerObject.tag = "Player";
            playerBody.GetComponent<MeshRenderer>().material = materials[0];
        }
    }
        private void invisible()
    {
        if (magicTank.magicMeter > 0)
        playerObject.tag = "Invisible";
        int invislayer = LayerMask.NameToLayer("Ignore Raycast");
        playerObject.layer = invislayer;
        playerBody.GetComponent<MeshRenderer>().material = materials[1];


    }
}
