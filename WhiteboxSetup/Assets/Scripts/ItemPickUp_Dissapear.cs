using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp_Dissapear : MonoBehaviour
{
    public LayerMask uhm;
    public GameObject PickText;
    public GameObject self;
    // Start is called before the first frame update
    void Start()
    {
        PickText.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
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
                        Destroy(self,0.3f);
                    }
                }
            }
        }
    }
}
