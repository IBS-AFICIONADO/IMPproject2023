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
    public ParticleSystem shoot;
    public ParticleSystem hit;
    public Material[] laserMaterials;
    private bool available;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        maxDist = manager.visionRadius + distance;
        mask = LayerMask.GetMask("target", "obstacle");

        StartCoroutine(shootPlayer());

    }

    // Update is called once per frame
    void Update()
    {
        var particleMain = charging.main;

        if (!manager.playerInFOV)
        {
            lineRenderer.enabled = false;
        }

        switch (manager.alertStage)
        {
            case AlertStage.Peaceful:
                lineRenderer.enabled = false;
                charging.Pause(true);
                charging.Clear(true);
                break;

            case AlertStage.IntriguedL1 when manager.playerInFOV == true:

                renderLaser(LaserWidth / 2, laserMaterials[0], false);
                if (!charging.isPlaying)
                {
                    charging.Play(true);
                }
                particleMain.simulationSpeed = Mathf.Lerp(1f, 5f, manager.alertLevel / manager.maxAlertEdit);
                break;

            case AlertStage.IntriguedL2 when manager.playerInFOV:
                renderLaser(LaserWidth / 2, laserMaterials[0], false);
                particleMain.simulationSpeed = Mathf.Lerp(1f, 5f, manager.alertLevel / manager.maxAlertEdit);
                break;

            case AlertStage.Alerted when !available && manager.playerInFOV:
                charging.Play(true);
                renderLaser(LaserWidth / 2, laserMaterials[0], false);
                particleMain.simulationSpeed = 5f;
     
                break;

            case AlertStage.Alerted when available && manager.playerInFOV:
                charging.Pause(true);
                charging.Clear(true);
                renderLaser(LaserWidth, laserMaterials[1], true);
                break;
           
        }

    }





    private IEnumerator shootPlayer()
    {
        WaitForSeconds cooldown = new WaitForSeconds(2f);
        WaitForSeconds shootDelay = new WaitForSeconds(secondsTillShoot);
        while (true)
        {
            yield return shootDelay;
            available = true;
            yield return new WaitForSeconds(.5f);
            if (lineRenderer.enabled)
            {
                charging.Clear();
                available = false;
            }
            yield return cooldown;
            available = true;

        }



    }

   private void renderLaser(float laserSize,Material material, bool isDeath)
    {
        Vector3 point;
        updateLength(isDeath);
        lineRenderer.startWidth = laserSize;
        lineRenderer.endWidth = laserSize;
        lineRenderer.material = material;
        lineRenderer.enabled = true;
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

   private void updateLength(bool death)
   {
        RaycastHit[] hit;
        directionToTarget = (manager.targetRef.transform.position - transform.position).normalized;
        hit = Physics.RaycastAll(transform.position, directionToTarget, maxDist, mask);
        int i = 0;
        while (i < hit.Length)
        {
            if (hit[i].collider.CompareTag("Player") && death)
            {
                Debug.Log("game over >_<");
            }

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

