using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp_Dissapear : MonoBehaviour
{
    public GameObject PickText;
    // Start is called before the first frame update
    void Start()
    {
        PickText.SetActive(false);
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PickText.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                PickText.SetActive(false);
            }
        }
    }
}
