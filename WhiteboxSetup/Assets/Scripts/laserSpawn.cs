using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserSpawn : MonoBehaviour
{
    public float LaserWidth;
    public Color laserColor;
    public RobotManager manager;
    [SerializeField]
    private float distance;
    private LineRenderer lineRenderer;
    private int length;
    private Vector3[] position;
    private LayerMask mask;
    private Vector3 directionToTarget;
    private float maxDist;
    public float secondsTillShoot;
    public ParticleSystem charging;
    private ParticleSystem uhm;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = LaserWidth;
        lineRenderer.endWidth = LaserWidth;
        maxDist = manager.visionRadius + distance;
        mask = LayerMask.GetMask("target", "obstacle");


    }

    // Update is called once per frame
    void Update()
    {
    if(manager.alertStage == AlertStage.IntriguedL2 || manager.alertStage == AlertStage.Alerted)
        {
            StartCoroutine(shootPlayer());
        }
  
    }


    private IEnumerator shootPlayer()
    {
        WaitForSeconds shootDelay = new WaitForSeconds(secondsTillShoot);
        while(manager.alertStage == AlertStage.IntriguedL2)
        {
            if (uhm == null)
            {
                uhm = Instantiate(charging, transform);
            }
            yield return null;
        }
        while(manager.alertStage == AlertStage.Alerted)
        {
            yield return shootDelay;
            lineRenderer.enabled = true;
            renderLaser();
            
            yield return null;
        }
        while (true)
        {
            lineRenderer.enabled = false;

            Destroy(uhm);
            yield return null;
        }

    }

    void renderLaser()
    {
        Vector3 point;
        updateLength();
        lineRenderer.startColor = laserColor;
        for(int i = 0; i < length; i++)
        {
            point.x = transform.position.x + i * directionToTarget.x;
            point.y = transform.position.y + i * directionToTarget.y;
            point.z = transform.position.z + i * directionToTarget.z;
            position[0] = transform.position;
            position[i] = point;
            lineRenderer.SetPosition(i, point);
        }

    }

    void updateLength()
    {
        RaycastHit[] hit;
        directionToTarget = (manager.targetRef.transform.position - transform.position).normalized;
        hit = Physics.RaycastAll(transform.position, directionToTarget, maxDist, mask);
        int i = 0;
        while (i < hit.Length)
        {
            if (!hit[i].collider.isTrigger)
            {
                length = (int)Mathf.Round(hit[i].distance) + 2;
                position = new Vector3[length];
                lineRenderer.positionCount = length;
                return;
            }
            i++;
        }
    }
}
