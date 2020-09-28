using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WayPointSystem))]
public class WayPointSystemEditor : Editor
{
   private WayPointSystem waypointSystem;
   public float arrowSize = 1;
   Vector3 cubeSize = new Vector3(1,1,1);

   private void OnEnable()
   {
      waypointSystem = target as WayPointSystem;
   }

   private void OnSceneGUI()
   {
      for (int i = 0; i < waypointSystem.points.Length; i++)
      {
         Handles.Label(waypointSystem.points[i], "WayPoint: "+i.ToString(), new GUIStyle { normal = new GUIStyleState() { textColor = Color.white }, alignment = TextAnchor.MiddleCenter, fontSize = 10 });
         waypointSystem.points[i] = Handles.DoPositionHandle(waypointSystem.points[i], Quaternion.identity);
         //Handles.SphereHandleCap(0, waypointSystem.points[i], Quaternion.identity, waypointSystem.radius, EventType.Repaint);
         Handles.DrawWireCube(waypointSystem.points[i], cubeSize*waypointSystem.radius);
         Vector3 next;
         if (i+1 < waypointSystem.points.Length)
         {
            next = waypointSystem.points[i + 1];
         }
         else
         {
            next = waypointSystem.points[0];
         }
         Handles.DrawLine(waypointSystem.points[i], next);
      }
   }
}
