using UnityEngine;

namespace DefaultNamespace
{
    public class DebugCMD : MonoBehaviour
    {
        private void OnGUI()
        {
            GUILayout.Label("curStage:" + GameMgr.Inst.mCurStage.ToString());
        }
    }
}