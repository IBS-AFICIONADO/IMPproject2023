using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class patrol : MonoBehaviour
{
    public RobotManager robot;
   // [SerializeField]
    //private float maxPathlength = 20;
    [SerializeField]
    private float patrolRadius;
    public int pointAmount;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (robot.wasChasing)
            createWaypoints();
       /* NavMeshPath[] array = createWaypoints();
        for (int i = 0; i < array.Length - 1; i++) {
            for (int j = 0; j < array[i].corners.Length - 1; j++)
                Debug.DrawLine(array[i].corners[j], array[i].corners[j + 1], Color.red);
        }*/
    }

    public NavMeshPath[] createWaypoints()
    {
        
        Vector3 startPos = robot.agent.destination;
        Vector3[] patrolPoints = new Vector3[pointAmount];
        NavMeshPath[] path = new NavMeshPath[pointAmount];
        for (int i = 0; i <= pointAmount; i++)
        {
            float randX = Random.Range(5, patrolRadius / 2);
            float randZ = Random.Range(5, patrolRadius / 2);
            patrolPoints[i] = new Vector3(startPos.x+randX,0,startPos.z+randZ);
            if (i > 0)
            {
                if (NavMesh.CalculatePath(patrolPoints[i - 1], patrolPoints[i], NavMesh.AllAreas, path[i]))
                {
                    NavMesh.CalculatePath(patrolPoints[i - 1], patrolPoints[i], NavMesh.AllAreas, path[i]);
                }
                else
                    i -= 1;
            }
            else
            {
                if (NavMesh.CalculatePath(startPos, patrolPoints[i], NavMesh.AllAreas, path[i]))
                {
                    NavMesh.CalculatePath(startPos, patrolPoints[i], NavMesh.AllAreas, path[i]);
                }
                else
                    i -= 1;
            }

        }
        return path;
    }
}
