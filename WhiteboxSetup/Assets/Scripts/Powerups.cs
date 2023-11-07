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
    public EnemyStun stunGrenade;

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
        if (magicTank == null)
        {
            magicTank = new magicTank();
            Debug.Log("new Tank");
        }
        
        if (Input.GetKey(KeyCode.Space) && magicTank.magicMeter > 1 && !magicTank.cooldown)
        {
            invisible();
            magicTank.deplete = true;

        }
        else
        {
            magicTank.deplete = false;
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
        }

        

       
        Debug.Log("MM" + magicTank.magicMeter);
    }
    //When player becomes invisible
    private void invisible()
    {
        if (magicTank.magicMeter > 0)
        {
            playerObject.tag = "Invisible";
            playerBody.GetComponent<MeshRenderer>().material = materials[1];
        }
       

    }
}
