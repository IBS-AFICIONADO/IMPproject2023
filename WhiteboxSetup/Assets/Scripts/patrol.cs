using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class patrol : MonoBehaviour
{
    public RobotManager robot;
    public float patrolRange;
    public int pointAmount;
    public Vector3[] idkman;
    private bool showPoints = false;
    void Start()
    {
        
    }
    private void Update()
    {
        if (robot.playerWasLost)
        {
            robot.patrolPoints = patrolPoints();
            idkman = robot.patrolPoints;
            robot.searching = true;
            showPoints = true;
        }

        if (showPoints)
        {
            for (int i = 1; i < idkman.Length; i++)
            {
                
                Debug.DrawRay(idkman[i], Vector3.up*2, Color.red);
               // Debug.Log(idkman[i]+" "+i);
            }
        }
    }
    private Vector3[] patrolPoints()
    {
        Vector3 [] patrolPoints = new Vector3[pointAmount+1];
        Vector3 playLastSeen = robot.agent.destination;
        int arrayIndex = 1;
        patrolPoints[0] = playLastSeen;
        int walkMask = 1 << NavMesh.GetAreaFromName("walkable");
            for (int i = 1; i < 50; i++)
            {
                Debug.Log("erm");
                NavMeshHit hit;
                Vector3 pointProposal = playLastSeen + Random.insideUnitSphere * patrolRange;
                if (NavMesh.SamplePosition(pointProposal, out hit, 1.84f*2f, NavMesh.AllAreas) && arrayIndex < pointAmount+1)
                {
                    patrolPoints[arrayIndex] = hit.position;
                //Debug.Log("success " + hit.position + " " + arrayIndex+" "+patrolPoints.Length);
                Debug.Log("success");
                    arrayIndex++;
                }
            else
                {
                    Debug.Log("fail");
                }
                    
            }
        
        return patrolPoints;
    }
      
}
