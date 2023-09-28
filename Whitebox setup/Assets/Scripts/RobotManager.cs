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
    //parameters vision
    public float radius;
    [Range(0, 360)] public float fovAngle;

    //parameters before enum change
    const float maxAlert = 100f;
    public float alertSpeed = 1;
    public float forgetSpeed = 1;
    

    public AlertStage alertStage;
    [Range(0, maxAlert)] public float alertLevel;

    //parametrs for raycasting, detecting objects
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    
  

    //parameters for changing UI above head enemies
    //public GameObject alertUI;
   // public Slider alertSlider;
   // public Image alertSliderFill;

    //parameters enemy movement
    public float turnRate;
    public float speed;

    //parameters needed in the editor but not important fror changing in editor
    [HideInInspector]
    public bool playerInFOV;
    [HideInInspector]
    public float maxAlertEdit = maxAlert;
    [HideInInspector]
    public GameObject targetRef;
    private void Awake()
    {
        alertStage = AlertStage.Peaceful;
        alertLevel = 0;
     //   alertSlider.value = 0;
        targetRef = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        StartCoroutine(FOVRoutine());
 
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return wait;
            FOVCheck();
            UpdateAlertstate(playerInFOV);
         
           RobotMovement(targetRef);

        }
    }
    private void FOVCheck()
    {
        Collider[] targetsInFOV = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (targetsInFOV.Length != 0)
        {
            foreach (Collider c in targetsInFOV)
            {
                if (c.CompareTag("Player"))
                {
                    Vector3 directionToTarget = (c.transform.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, c.transform.position);
                        if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        {
                            playerInFOV = true;
                        }
                        else
                            playerInFOV = false;
                    }
                    else
                        playerInFOV = false;
                }   
            }
        }
        else if (playerInFOV)
            playerInFOV = false;
    }
  
    private void UpdateAlertstate(bool playerinFOV)
    {
        switch (alertStage)
        {
            case AlertStage.Peaceful:
                alertLevel = 0;
                if (playerinFOV)
                    alertStage = AlertStage.Intrigued;
                break;

            case AlertStage.Intrigued:
                if (playerinFOV)
                {
                    alertLevel+= alertSpeed * Time.deltaTime;
                    if (alertLevel >= maxAlert)
                    {
                        alertStage = AlertStage.Alerted;
                    }
                }
                else
                {
                    alertLevel-= forgetSpeed * Time.deltaTime;
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
                else
                    alertLevel = 100;
                break;

        }
    }
    private void RobotMovement(GameObject target)
    {
        if (alertStage == AlertStage.Alerted)
        {
            var toPlayer = target.transform.position - transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                Quaternion.LookRotation(toPlayer),
                Time.deltaTime * turnRate);
           // transform.Translate(transform.forward * speed * Time.deltaTime);
        }
    }
}

