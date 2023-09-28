using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RobotManager))]
public class RobotManagerEditor : Editor
{
    private void OnSceneGUI()
    {
        RobotManager fov = (RobotManager)target;

        //drawing circle with radius of viewing radius
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        //setting color to alertness level and drawing vision cone
        Color c = Color.green;
        if (fov.alertStage == AlertStage.Intrigued)
        {
            c = Color.Lerp(Color.yellow, Color.red, fov.alertLevel / fov.maxAlertEdit);
        }
        else if (fov.alertStage == AlertStage.Alerted)
        {
            c = Color.red;
        }
        Handles.color = new Color(c.r, c.g, c.b, 0.1f);
        Handles.DrawSolidArc(fov.transform.position, fov.transform.up,
            Quaternion.AngleAxis(-fov.fovAngle / 2f, fov.transform.up) * fov.transform.forward,
            fov.fovAngle, fov.radius);

        //drawing raycastline 
        if (fov.playerInFOV)
        {
            Handles.color = Color.cyan;
            Handles.DrawLine(fov.transform.position, fov.targetRef.transform.position);
        }



    }
}
  