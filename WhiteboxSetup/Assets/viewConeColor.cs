using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewConeColor : MonoBehaviour
{
    public RobotManager manager;
    private MeshRenderer meshRenderer;
    private Color c;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color yellow = new Color(1, 0.92f, 0.016f, 0.5f);
        Color red = new Color(1, 0, 0, 0.5f);
        Color green = new Color(0, 1, 0, 0.5f);
        if(manager.alertStage == AlertStage.Peaceful)
        {
            c = green;
        }
        else if (manager.alertStage == AlertStage.IntriguedL1 || manager.alertStage == AlertStage.IntriguedL2)
        {
            c = Color.Lerp(yellow, red, manager.alertLevel / manager.maxAlertEdit);
        }
        else if (manager.alertStage == AlertStage.Alerted)
        {
            c = red;
        }
        meshRenderer.material.SetColor("_Color", c);
    }
}
