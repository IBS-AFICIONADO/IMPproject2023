using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Image chargeIndicator = null;
    private bool shouldUpdate;
    public float fill;
    private void Start()
    {
        
    }

    private void Update()
    {
        fill = GameObject.Find("Player").GetComponent<playerController>().invisibleTimer;
        if (GameObject.Find("Player").GetComponent<playerController>().invisibleTimer < 100)
        {
            chargeIndicator.fillAmount = fill / 100;
        }
    }
}
