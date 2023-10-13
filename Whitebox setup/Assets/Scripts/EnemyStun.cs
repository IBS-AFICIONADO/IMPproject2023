using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStun : MonoBehaviour
{
    public GameObject stunGrenade;
    public float throwForce;
    public Transform cam;
    public GameObject spawner;

     // Start is called before the first frame update
     void Awake()
     {


        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            throwProjectile();
        }
    }

    private void throwProjectile()
    {
        GameObject projectile = Instantiate(stunGrenade, spawner.transform);
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
        projectileRB.velocity = Vector3.zero;
        projectileRB.angularVelocity = Vector3.zero;
        projectileRB.isKinematic = false;
        projectileRB.transform.SetParent(null, true);
        projectileRB.AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
    }
}
