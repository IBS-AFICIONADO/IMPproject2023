using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    public float turnRate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up, turnRate * Input.GetAxis("Horizontal") * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * Input.GetAxis("Vertical") * Time.deltaTime);

    }
}
