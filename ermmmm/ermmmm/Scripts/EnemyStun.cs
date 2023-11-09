using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStun : MonoBehaviour
{
    public GameObject stunGrenade;
    public float throwForce;
    public Transform cam;

    private GameObject projectile;
    private Rigidbody projectileRB;

    public bool available = true;
    [SerializeField]
    public LineRenderer LineRenderer;
    [SerializeField]
    [Range(10, 100)]
    private int LinePoints;
    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float timeBetweenpoints;
    public float upforce;
    private LayerMask collisonMask;
    // Start is called before the first frame update
    void Awake()
    {
        LineRenderer.enabled = false;
        int grenadeLayer = stunGrenade.gameObject.layer;

         for(int i=0; i < 32; i++)
        {
            if (!Physics.GetIgnoreLayerCollision(grenadeLayer, i))
            {
                collisonMask |= 1 << i;
            }
        }
    }

    // Update is called once per frame
    public void throwProjectile()
    {

        available = true;
        projectileRB.velocity = Vector3.zero;
        projectileRB.angularVelocity = Vector3.zero;

        projectileRB.constraints = RigidbodyConstraints.None;
        projectileRB.freezeRotation = true;
        projectileRB.isKinematic = false;
        projectileRB.transform.SetParent(null, true);
        projectileRB.AddForce(cam.transform.forward * throwForce + upforce * transform.up, ForceMode.Impulse);
    }

    public void holdProjectile()
    {

        if (available)
        {
            projectile = Instantiate(stunGrenade, transform);
            projectileRB = projectile.GetComponent<Rigidbody>();
            projectileRB.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void drawProjection()
    {
        LineRenderer.enabled = true;
        LineRenderer.positionCount = Mathf.CeilToInt(LinePoints / timeBetweenpoints) + 1;
        Vector3 startPosition = transform.position;
        Vector3 startVelocity = throwForce * cam.transform.forward + upforce * transform.up / projectileRB.mass;
        int i = 0;
        LineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < LinePoints; time += timeBetweenpoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            LineRenderer.SetPosition(i, point);

            Vector3 lastPosition = LineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized,out RaycastHit hit, (point - lastPosition).magnitude,collisonMask))
            {
                LineRenderer.SetPosition(i, hit.point);
                LineRenderer.positionCount = i + 1;
                return;
            }
        }
    }

}


