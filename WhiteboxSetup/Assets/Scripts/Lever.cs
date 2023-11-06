using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public LayerMask uhm;
    public GameObject PickText;
    public GameObject self;

    void Start()
    {
        PickText.SetActive(false);
    }
    // Start is called before the first frame update
    void update()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, 5, uhm);
        if (targets.Length != 0)
        {
            foreach (Collider c in targets)
            {
                if (c.CompareTag("Player"))
                {
                    PickText.SetActive(true);
                    if (Input.GetKey(KeyCode.E))
                    {
                        PickText.SetActive(false);
                        self.transform.Rotate(0,0,0.90f,Space.Self);
                    }
                }
            }
        }
    }
}
