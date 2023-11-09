using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [Range(0, 100)] public float invisibleTimer;
    [SerializeField]
    private Material[] materials = new Material[2];
    private bool cooldown = false;
    // Start is called before the first frame update
    void Start()
    {
        playerBody.GetComponent<MeshRenderer>().material = materials[0];
        invisibleTimer = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (invisibleTimer <= 2)
        {
            StartCoroutine(cooldownTimer());
        }
        if (Input.GetKey(KeyCode.Space) && invisibleTimer > 1 && !cooldown)
        {
            invisible();
        }
        else
        {
            playerObject.tag = "Player";
            playerBody.GetComponent<MeshRenderer>().material = materials[0];
            if (invisibleTimer < 100 )
            invisibleTimer += invisCharge * Time.deltaTime;
        }
    
        

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Robot"))
        {
            SceneManager.LoadScene("GameOver");
                }
    }
    private void invisible()
    {
        if(invisibleTimer>0)
        invisibleTimer -= invisDeplete * Time.deltaTime;
        playerObject.tag = "Invisible";
        playerBody.GetComponent<MeshRenderer>().material = materials[1];


    }
    private IEnumerator cooldownTimer()
    {
        WaitForSeconds cooldownTime = new WaitForSeconds(3f);
        yield return null;
        cooldown = true;
        yield return cooldownTime;
        cooldown = false;
        yield break;
    }
}
