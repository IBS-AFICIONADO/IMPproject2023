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
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(parent);
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
        WaitForSeconds wait = new WaitForSeconds(0.02f);
        while (radius > effectAreaPrefab.transform.localScale.x)
        {
            yield return wait;
                effectAreaPrefab.transform.localScale += Vector3.one * Time.deltaTime * expandFactor;  
        }
        yield return null;

    }
}
