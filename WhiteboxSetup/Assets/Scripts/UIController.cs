using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Image chargeIndicator = null;
    public magicTank magicTank;

    private void Update()
    {
        if (magicTank.magicMeter < 100)
        {
            chargeIndicator.fillAmount = magicTank.magicMeter / 100;
        }
    }
}
