using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    public GameObject PickupText;
    // Start is called before the first frame update
    void Start()
    {
        PickupText.SetActive(false);
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") {

            PickupText.SetActive(true);

            if(Input.GetKeyUp(KeyCode.E)) {
                this.gameObject.SetActive(false);
            }
        }
    }
}
