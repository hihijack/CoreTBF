using System;
using UnityEngine;

public class DebugCMD : MonoBehaviour
    {
        public bool showGMWindow = false;
        Rect windowrect=new Rect(0,0,120,50);

        private void OnGUI()
        {
            if (showGMWindow)
            {
                windowrect = GUI.Window(0, windowrect, DrawWindow, "CMD");
            }
        }

    private void DrawWindow(int id)
    {
       if (GUILayout.Button("下一层"))
       {
            EventProcessor.Inst.FireEvent(EventProcessor.EVENT_TO_NEXT_AREA, null, null);    
       }

       GUI.DragWindow();
    }
}