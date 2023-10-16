using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadeController : MonoBehaviour
{
    public GameObject parent;
    public GameObject effectArea;
    public float radius;
    public float expandFactor;
    private bool exploded = false;
    private GameObject effectAreaPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (!exploded)
        {
            exploded = true;
            effectAreaPrefab = Instantiate(effectArea, parent.transform.position, Quaternion.identity);
            effectAreaPrefab.transform.SetParent(null, false);
            StartCoroutine(explode());
        }


    }
    private IEnumerator explode()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        while (radius > effectAreaPrefab.transform.localScale.x)
        {
            effectAreaPrefab.transform.localScale += Vector3.one * Time.deltaTime * expandFactor;
            yield return wait;
        }
        while (radius <= effectAreaPrefab.transform.localScale.x)
        {
            Destroy(effectAreaPrefab);
            Destroy(parent);
            yield return null;
        }

    }
}
