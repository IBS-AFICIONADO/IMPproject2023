using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum AlertStage
{
    Peaceful,
    IntriguedL1,
    IntriguedL2,
    Alerted

}

public class RobotManager : MonoBehaviour
{
    [Header("vision and hearing")]
    //parameters vision
    public float visionRadius;
    [Range(0, 360)] public float fovAngle;

    //hearing + powerup stuff
    public float hearingRadius;

    //parameters before enum change
    const float maxAlert = 100f;
    [SerializeField]
    private LayerMask targetMask;
    public LayerMask obstructionMask;

    [Header("alerting and forgetting")]
    public float alertSpeed;
    public float forgetSpeed;
    public AlertStage alertStage;
    [Range(0, maxAlert)] 
    public float alertLevel;


    [Header("visioncone settings")]
    //parameters for drawing vision cone
    [SerializeField]
    private float meshResolution;
    [SerializeField]
    private MeshFilter viewMeshFilter;
    private Mesh viewMesh;
    [SerializeField]
    private int edgeDetectIte;
    [SerializeField]
    private float edgeDistThreshold;
    public bool visioncone;

    //parameters needed in the editor but not important for changing in unity inspector
    [HideInInspector]
    public bool playerInFOV;
    [HideInInspector]
    public float maxAlertEdit = maxAlert;
    [HideInInspector]
    public Collider targetRef;
    
    //variables for getting hit with stun
    [Header("stun settings")]
    [SerializeField]
    private float stunTime;
    private bool isStunned;
    private float initialRadius;
    
    [Header("AI pathfinding")]
    //For the waypoint
    public Transform[] routinePoints;
    [HideInInspector]
    public Vector3[] patrolPoints;
    [HideInInspector]
    public bool searching = false;
    private int m_CurrentWaypointIndex;

    [SerializeField]
    private float alertVel;

    [SerializeField]
    private float intrigueVel;

    [SerializeField]
    private float patrolVel;

    [HideInInspector]
    public bool playerWasLost;
    private int waypointIndex;
    //moving AI bits
    public NavMeshAgent agent;
    private Vector3 player;


    //animation
    private CharacterController CharacterController;
    private Animator animator;

    private void Awake()
    {
        alertStage = AlertStage.Peaceful;
        alertLevel = 0;
        agent = GetComponent<NavMeshAgent>();
        GameObject targetRefObject = GameObject.FindGameObjectWithTag("Player");
        targetRef = targetRefObject.GetComponent<CapsuleCollider>();

        viewMesh = new Mesh();
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine(robotRoutine());

        if (routinePoints.Length > 0)
            agent.SetDestination(routinePoints[0].position);

        animator = GetComponent<Animator>();
        CharacterController = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Stun"))
            {
                StartCoroutine(stunned());
            }
        }


    private void LateUpdate()
    {
        //needs to be as late update because______ dont move :)
        drawFOV();

    }

    private void animationBools()
    {
        if(agent.velocity != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        animator.SetBool("isStunned", isStunned);
    }

    private IEnumerator robotRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            FOVCheck();
            UpdateAlertstate(playerInFOV);
            moveToWaypoints();
            animationBools();
        }
    }

    private IEnumerator stunned()
        {
        
            WaitForSeconds stunCooldown = new WaitForSeconds(stunTime);
            while (!isStunned)
            {
                initialRadius = visionRadius;
                visionRadius = 0;

                agent.ResetPath();
                agent.velocity = Vector3.zero;
                isStunned = true;

                alertStage = AlertStage.Peaceful;
                alertLevel = 0;
                visionRadius = 0;
                yield return null;
            }
    
            yield return new WaitForSeconds(stunTime);
        
            while (isStunned)
            {
                visionRadius = initialRadius;
                Debug.Log(initialRadius); ;
                isStunned = false;
                agent.isStopped = false;
                yield return null;
            }
            yield return stunCooldown;
        }
   
    private void FOVCheck()
    {
        //check if there are any other colliders in a sphere with view radius on the targetlayer specific layer prevents from scanning all layers every iteration
        Collider[] targetsInFOV = Physics.OverlapSphere(transform.position, visionRadius, targetMask);
        if (targetsInFOV.Length != 0)
        {
            foreach (Collider c in targetsInFOV)
            {
                if (c.CompareTag("Player"))
                {

                    //calculate direction between enemy and player
                    Vector3 directionToTarget = (c.transform.position - transform.position).normalized;

                    //angle returns the smallest angle between two vectors so fovAngle has to be halved
                    if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, c.transform.position);
                        //raycast on the layer of obstacles if it returns from enemy to player if it hits nothing robot can see the player
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

    public void UpdateAlertstate(bool playerinFOV)
    {
        switch (alertStage)
        {
            case AlertStage.Peaceful:
                alertLevel = 0;
                if (playerinFOV)
                    alertStage = AlertStage.IntriguedL1;
                break;

            case AlertStage.IntriguedL1:
                playerWasLost = false;
                if (playerinFOV)
                {
                    alertLevel += alertSpeed;
                    if (alertLevel >= maxAlert/4)
                    {
                        alertStage = AlertStage.IntriguedL2;
                    }
                }
                else
                {
                    alertLevel -= forgetSpeed;
                    if (alertLevel <= 0)
                    {
                        alertStage = AlertStage.Peaceful;
                    }
                }
                break;

            case AlertStage.IntriguedL2:
                if (playerinFOV)
                {
                    alertLevel += alertSpeed;
                    if (alertLevel >= maxAlert)
                    {
                        alertStage = AlertStage.Alerted;
                    }
                }
                else
                {
                    alertLevel -= forgetSpeed;
                    if (alertLevel <= 0)
                    {
                        playerWasLost = true;
                        alertStage = AlertStage.IntriguedL1;
                    }
                }
                break;

            case AlertStage.Alerted:
                if (!playerinFOV)
                {
                    alertStage = AlertStage.IntriguedL2;
                }
                else
                    alertLevel = 100;
                break;

        }
    }
    
    private void moveToWaypoints()
    {
        if (!isStunned)
        {
            switch (alertStage)
            {
                case AlertStage.Alerted:
                    agent.speed = alertVel;
                    player = targetRef.transform.position;
                    if(targetRef.CompareTag("Player"))
                    agent.SetDestination(player);
                    break;

                case AlertStage.IntriguedL2:
                    agent.speed = intrigueVel;
                    player = targetRef.transform.position;
                    if (targetRef.CompareTag("Player"))
                        agent.SetDestination(player);
                    break;

                case AlertStage.IntriguedL1:
                    if (!searching)
                    {
                        agent.speed = patrolVel;
                        regularPatrol();
                    }
                    else
                    {
                        agent.speed = intrigueVel;
                        searchPatrol();
                    }
                    break;

                case AlertStage.Peaceful:
                    if (!searching)
                    {
                        agent.speed = patrolVel;
                        regularPatrol();
                    }
                    else
                    {
                        agent.speed = intrigueVel;
                        searchPatrol();
                    }
                    break;
            }         
        }
    }

    private void regularPatrol()
    {
        if (agent.remainingDistance < agent.stoppingDistance && routinePoints.Length > 0)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % routinePoints.Length;
            agent.SetDestination(routinePoints[m_CurrentWaypointIndex].position);
        }
    }

    private void searchPatrol()
    {

        if (agent.remainingDistance < agent.stoppingDistance && waypointIndex < patrolPoints.Length - 1)
        {
            waypointIndex++;
            agent.SetDestination(patrolPoints[waypointIndex]);
        }

        if(waypointIndex == patrolPoints.Length - 1)
        {
            searching = false;
            waypointIndex = 0;
            agent.SetDestination(routinePoints[0].position);
        }
    }
   
    private void drawFOV()
    {
        //amount of rays cast is the angle* resolution
        int stepCount = Mathf.RoundToInt(fovAngle * meshResolution);
        float stepAngleSize = fovAngle / stepCount;
        //list that contains all vector3's returned by viewcast used for building the mesh
        List<Vector3> viewPoints = new List<Vector3>();

        ViewCastInfo oldCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++){
            float angle = transform.eulerAngles.y - fovAngle / 2 + stepAngleSize*i ;
            //every iteration see what viewcast returns for the given segment of the mesh
            ViewCastInfo newViewCast = ViewCast(angle);
            if (i > 0)
            {
                bool overThreshold = Mathf.Abs(oldCast.distance - newViewCast.distance) > edgeDistThreshold;
                if(oldCast.hit != newViewCast.hit || (oldCast.hit && newViewCast.hit && overThreshold))
                {
                    EdgeInfo edge = detectEdge(oldCast, newViewCast);
                    if(edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB!= Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.endPoint);
            oldCast = newViewCast;
        }

        //vertices + 1 amount of rays cast + origin vertex
        int vertexAmount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexAmount];
        //making a mesh is odd i can draw it if anyone is interested :3
        int[] triangles = new int[(vertexAmount - 2)*3];

        //mesh renderer will be a child of robotprefab -> robot pos for mesh is vector3.zero
        vertices[0] = Vector3.zero;
        for(int i=0; i < vertexAmount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexAmount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }


    //method that takes an angle in degrees between target  and origin converts it to a vector3 
    public Vector3 DirFromAngle(float angleInDeg, bool isGlobal)
    {
        if (!isGlobal)
        {
            angleInDeg += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
    }


    //manages sending out raycasts
    ViewCastInfo ViewCast(float angle)
    {
        Vector3 dir = DirFromAngle(angle, true);
        RaycastHit hit;
        //if raycast hits smt it returns position of hit + angle distance etc of raycast hit = true cause it hit smt in this case
        if(Physics.Raycast(transform.position,dir,out hit, visionRadius, obstructionMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, angle);
        } else
            //didnt hit anything so endpoint is direction * visionradius but from position of robot so+transform
            return new ViewCastInfo(false, transform.position+dir*visionRadius, visionRadius, angle);
    }
    
    //holds all relevant information for the viewcasting method that builds the mesh for visible viewcones
    public struct ViewCastInfo
    {
        //did raycast hit, endpoint of cast, length of cast, angle of ray
        public bool hit;
        public Vector3 endPoint;
        public float distance;
        public float angle;

        //constructor
        public ViewCastInfo(bool _hit, Vector3 _end, float _distance,float _angle)
        {
            hit = _hit;
            endPoint = _end;
            distance = _distance;
            angle = _angle;
        }
    }


    EdgeInfo detectEdge(ViewCastInfo minimum, ViewCastInfo maximum)
    {
        float minAngle = minimum.angle;
        float maxAngle = maximum.angle;
        Vector3 minEndPoint = Vector3.zero;
        Vector3 maxEndPoint = Vector3.zero;

        //every loop cast a ray in the middle of the max and min ray to see if in order to detect edges
        for(int i = 0; i < edgeDetectIte; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool overThreshold = Mathf.Abs(minimum.distance - newViewCast.distance) > edgeDistThreshold;

            if (newViewCast.hit == minimum.hit && !overThreshold)
            {
                minAngle = angle;
                minEndPoint = newViewCast.endPoint;
            }
            else
            {
                maxAngle = angle;
                maxEndPoint = newViewCast.endPoint;
            }
        }
        return new EdgeInfo(minEndPoint, maxEndPoint);
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}