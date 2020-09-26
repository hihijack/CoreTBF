using UnityEngine;

namespace UI
{
    public class UIMgr : MonoBehaviour
    {
        public UIFight uiFight;
        public UIFightLog uiFightLog;

        public UIFightActionRoot uiFightActionRoot;

        public UIHPRoot uiHPRoot;

        private static UIMgr _inst;
        
        public static UIMgr Inst
        {
            get { return _inst; }
        }

        private void Awake()
        {
            _inst = this;
        }

        private void Start()
        {
            uiFightActionRoot.SetVisible(false);
        }
    }
}