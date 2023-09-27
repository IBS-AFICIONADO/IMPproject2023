using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AlertStage
{
    Peaceful,
    Intrigued,
    Alerted

}
public class RobotManager : MonoBehaviour
{
    public float fov;
    public AlertStage alertStage;
    const float maxAlert = 100f;
    [Range(0, 360)] public float fovAngle;
    [Range(0, maxAlert)] public float alertLevel;
    public float maximumAlert = maxAlert;

    public GameObject alertUI;
    public Slider alertSlider;
    public Image alertSliderFill;

    public float turnRate;
    public float speed;

    private void Awake()
    {
        alertStage = AlertStage.Peaceful;
        alertLevel = 0;
        alertSlider.value = 0;
    }

    private void Update()
    {
        bool playerInFOV = false;
        Collider[] targetsInFOV = Physics.OverlapSphere(
            transform.position, fov);
        
        foreach(Collider c in targetsInFOV)
        {
            if (c.CompareTag("Player"))
            {
                float signedAngle = Vector3.Angle(transform.forward, c.transform.position - transform.position);
                if (Mathf.Abs(signedAngle) < fovAngle/2)
                    playerInFOV = true;
                break;
            }
        }

        Alertslider();
        UpdateAlertstate(playerInFOV);
        RobotMovement(targetsInFOV);
 
    }

    private void Alertslider()
    {
        alertUI.SetActive(false);
        if (alertLevel != 0)
        {
            alertUI.SetActive(true);
            alertSlider.value = alertLevel / maxAlert;
            alertSliderFill.color = Color.Lerp(Color.yellow, Color.red, alertSlider.value);
        }
    }

    private void UpdateAlertstate(bool playerinFOV)
    {
        switch (alertStage)
        {
            case AlertStage.Peaceful:
                if (playerinFOV)               
                    alertStage = AlertStage.Intrigued;             
                break;

            case AlertStage.Intrigued:
                if (playerinFOV)
                {
                    alertLevel ++ ;
                    if (alertLevel >= maxAlert)
                    {
                        alertStage = AlertStage.Alerted;
                    }
                }
                else
                {
                    alertLevel--;
                    if (alertLevel <= 0)
                    {
                        alertStage = AlertStage.Peaceful;
                    }
                }
                break;

            case AlertStage.Alerted:
                if (!playerinFOV)
                {
                    alertStage = AlertStage.Intrigued;
                }
                break;
           
        }
    }

    private void RobotMovement(Collider[] colliders)
    {
        if (alertStage == AlertStage.Alerted)
        {
            foreach(Collider c in colliders)
            {
                var toPlayer = c.transform.position - transform.position;
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    Quaternion.LookRotation(toPlayer),
                    Time.deltaTime * turnRate);
                transform.Translate(transform.forward * speed * Time.deltaTime);

            }
        }
    }
}

