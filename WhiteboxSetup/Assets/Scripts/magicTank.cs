using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicTank : MonoBehaviour
{
   
    [SerializeField]
    private float magicDeplete;
    [SerializeField]
    private float magicCharge;
    [Range(0, 100)] public float magicMeter;
    public bool cooldown = false;
    public bool deplete = false;
    // Start is called before the first frame update
    void Start()
    {
        magicMeter = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (magicMeter <= 2)
        {
            StartCoroutine(cooldownTimer());
        }
        if (deplete)
        {
            magicMeter -= magicDeplete * Time.deltaTime;
        }
        else if(magicMeter < 100)
        {
            magicMeter += magicCharge * Time.deltaTime;
        }
    
        

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
