using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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

    //parameters needed in the editor but not important for changing in editor
    [HideInInspector]
    public bool playerInFOV;
    [HideInInspector]
    public float maxAlertEdit = maxAlert;
    [HideInInspector]
    public GameObject targetRef;

    //moving AI bits
    NavMeshAgent agent;
    private Vector3 player;
    
    //on start all enemies are set to peaceful 
    private void Awake()
    {
        alertStage = AlertStage.Peaceful;
        alertLevel = 0;
        agent = GetComponent<NavMeshAgent>();
        targetRef = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        //by using a coroutine that runs every .5 seconds load is lower 
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
            moveTo();
        }
    }

    private void FOVCheck()
    {
        //check if there are any other colliders in a sphere with view radius on the targetlayer specific layer prevents from scanning all layers every iteration
        Collider[] targetsInFOV = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (targetsInFOV.Length != 0)
        {
            foreach (Collider c in targetsInFOV)
            {
                if (c.CompareTag("Player"))
                {
                    //calculate direction between enemy and player
                    Vector3 directionToTarget = (c.transform.position - transform.position).normalized;

                    //angle returns the smallest angle between two vectors sp fovAngle has to be halved
                    if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, c.transform.position);
                        //draws a raycast on the layer of obstacles if it returns from enemy to player if it hits nothing robot can see the player
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

               // if(c.CompareTag("Distraction"))
               // {

               // }
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
                agent.isStopped = true;
                if (playerinFOV)
                    alertStage = AlertStage.Intrigued;
                break;

            case AlertStage.Intrigued:
                if (playerinFOV)
                {
                    alertLevel += alertSpeed * Time.deltaTime;
                    if (alertLevel >= maxAlert)
                    {
                        alertStage = AlertStage.Alerted;
                    }
                }
                else
                {
                    alertLevel -= forgetSpeed * Time.deltaTime;
                    if (alertLevel <= 0)
                    {
                        alertStage = AlertStage.Peaceful;
                    }
                }
                break;

            case AlertStage.Alerted:

                agent.isStopped = false;
                agent.SetDestination(player);

                if (!playerinFOV)
                {
                    alertStage = AlertStage.Intrigued;
                }
                else
                    alertLevel = 100;
                break;

        }
    }
    
    private void moveTo()
    {
        if (alertStage == AlertStage.Alerted)
        {
            player = targetRef.transform.position;
            agent.isStopped = false;
            agent.SetDestination(player);
        } else if (alertStage == AlertStage.Intrigued && !agent.isStopped)
        {
            player = targetRef.transform.position;
            agent.SetDestination(player);
        }
        else if(alertStage == AlertStage.Peaceful)
            agent.isStopped = true;
    }
}