using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RobotManager))]
public class RobotManagerEditor : Editor
{
 private void OnSceneGUI()
    {
        RobotManager robot = (RobotManager)target;
        Color c = Color.green;
        if(robot.alertStage == AlertStage.Intrigued)
        {
            c = Color.Lerp(Color.green, Color.red, robot.alertLevel / robot.maximumAlert);
        } else if (robot.alertStage == AlertStage.Alerted)
        {
            c = Color.red;
        }

        Handles.color = new Color(c.r, c.g, c.b,0.3f);
        Handles.DrawSolidArc(
            robot.transform.position,
            robot.transform.up,
           Quaternion.AngleAxis(-robot.fovAngle/2f,robot.transform.up)* robot.transform.forward,
            robot.fovAngle,
            robot.fov);
        Handles.color = c;
        robot.fov = Handles.ScaleValueHandle( 
            robot.fov,
            robot.transform.position + robot.transform.forward * robot.fov,
            robot.transform.rotation,
            3,
            Handles.ConeHandleCap, 1
            );
    }
}
