using System;
using UnityEditor;
using UnityEngine;

public class DebugCMD : MonoBehaviour
{
    public bool showGMWindow = false;
    Rect windowrect = new Rect(0, 0, 120, 180);

    string nextEventKey;

    /// <summary>
    /// 是否无敌
    /// </summary>
    public static bool isInvincible = false;

    const string KEY_NEXT_EVENT = "KEY_DEBUG_NEXT_EVENT";

    private void OnGUI()
    {
        if (showGMWindow)
        {
            windowrect = GUI.Window(0, windowrect, DrawWindow, "CMD");
        }
    }

    private void Awake()
    {
        nextEventKey = PlayerPrefs.GetString(KEY_NEXT_EVENT, "");
        WorldRaidData.Inst.DebugSetNextEventKey(nextEventKey);
    }

    private void DrawWindow(int id)
    {
        if (GUILayout.Button("下一层"))
        {
            EventProcessor.Inst.FireEvent(EventProcessor.EVENT_TO_NEXT_AREA, null, null);
        }

        if (GUILayout.Button("击败敌人"))
        {
            Event.Inst.Fire(Event.EEvent.FIGHT_WIN, null);
            GameMgr.Inst.ToState(EGameState.MainStage);
        }

        GUILayout.BeginVertical("box");
        GUILayout.Label("下一个事件ID");
        nextEventKey = GUILayout.TextField(nextEventKey);
        if (GUI.changed)
        {
            WorldRaidData.Inst.DebugSetNextEventKey(nextEventKey);
            PlayerPrefs.SetString(KEY_NEXT_EVENT, nextEventKey);
        }
        GUILayout.EndVertical();

        isInvincible = GUILayout.Toggle(isInvincible, "无敌");

        GUI.DragWindow();
    }
}