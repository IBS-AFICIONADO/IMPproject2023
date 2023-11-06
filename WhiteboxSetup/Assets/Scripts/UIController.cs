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
    public magicTank magicTank;

    private void Update()
    {
        if (magicTank.magicMeter < 100)
        {
            chargeIndicator.fillAmount = fill / 100;
        }
    }
}
