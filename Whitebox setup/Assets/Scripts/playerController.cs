using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private GameObject playerBody;
    [SerializeField]
    private float invisDeplete;
    [SerializeField]
    private float invisCharge;
    [Range(0, 100)]
    public float invisibleTimer = 100;
    [SerializeField]
    private Material[] materials = new Material[2];
    // Start is called before the first frame update
    void Start()
    {
        playerBody.GetComponent<MeshRenderer>().material = materials[0];
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Space) && invisibleTimer > 0)
        {
            invisible();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            playerObject.tag = "Player";
            playerBody.GetComponent<MeshRenderer>().material = materials[0];

        }
        invisibleTimer += invisCharge * Time.deltaTime;

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Robot"))
        {
            Debug.Log("game over :(");
        }
    }
    private void invisible()
    {
        invisibleTimer -= invisDeplete * Time.deltaTime;
        playerObject.tag = "Invisible";
        playerBody.GetComponent<MeshRenderer>().material = materials[1];

    }
}
