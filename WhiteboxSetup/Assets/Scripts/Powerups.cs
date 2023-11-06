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
        Debug.Log(magicTank.magicMeter);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && magicTank.magicMeter > 1 && !magicTank.cooldown)
        {
            invisible();
            magicTank.deplete = true;
            Debug.Log(magicTank.deplete);
        }
        else if (Input.GetMouseButton(1) && magicTank.magicMeter > 1 && !magicTank.cooldown)
        {

            stunGrenade.holdProjectile();
            stunGrenade.drawProjection();
            stunGrenade.available = false;
        }
        else if (Input.GetMouseButtonUp(1) && magicTank.magicMeter > 1 && !magicTank.cooldown)
        {
            stunGrenade.LineRenderer.enabled = false;
            stunGrenade.throwProjectile();
        }
        else
        {
            magicTank.deplete = false;
            playerObject.tag = "Player";
            playerBody.GetComponent<MeshRenderer>().material = materials[0];
        }
    }
    private void invisible()
    {
        if (magicTank.magicMeter > 0)
        {
            playerObject.tag = "Invisible";
            playerBody.GetComponent<MeshRenderer>().material = materials[1];
        }
       

    }
}
