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

    [SerializeField]
    public EnemyStun stunGrenade;

    [SerializeField]
    public AudioSource grenadeThrow;
   
    

    // Start is called before the first frame update

    void Start()
    {
        playerBody.GetComponent<MeshRenderer>().material = materials[0];
        

    }

    // Update is called once per frame
    void Update()
    {
        if (magicTank == null)
        {
            magicTank = new magicTank();
            Debug.Log("new Tank");
        }

        //Activation of Invisible with the key space, works only when mana is more than 1 and not on cooldown
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
        if (Input.GetMouseButton(1) && magicTank.magicMeter > 40 && !magicTank.cooldown)
        {
            stunGrenade.holdProjectile();
            stunGrenade.drawProjection();
            stunGrenade.available = false;
            magicTank.deplete = true;
        }
        if (Input.GetMouseButtonUp(1) && magicTank.magicMeter > 40 && !magicTank.cooldown)
        {
            stunGrenade.LineRenderer.enabled = false;
            stunGrenade.throwProjectile();
            magicTank.magicMeter -= 40;
            magicTank.deplete = true;
            grenadeThrow.Play();
            
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
