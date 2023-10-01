using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

public static class UIUtil
{
    public static Vector2 CalUIPosOfFight(Vector3 worldPos, float screenOffsetX, float screenOffsetY)
    {
        var screenPos = FightState.Inst.cameraMain.WorldToScreenPoint(worldPos) + new Vector3(screenOffsetX, screenOffsetY, 0);
        Vector2 locPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIFight.Inst.GetRect(), screenPos, null, out locPos);
        return locPos;
    }

    public static Vector2 CalUIPosOfUI(RectTransform rectTf, Vector3 worldPos, float screenOffsetX, float screenOffsetY)
    {
        var screenPos = FightState.Inst.cameraMain.WorldToScreenPoint(worldPos) + new Vector3(screenOffsetX, screenOffsetY, 0);
        Vector2 locPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTf, screenPos, null, out locPos);
        return locPos;
    }
}
