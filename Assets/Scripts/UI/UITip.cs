using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UITip : MonoBehaviour
    {
        public Text txtTip;

        private bool showing;

        public static UITip Inst { get; private set; }

        private void Awake()
        {
            Inst = this;
        }

        public void Show(string tip)
        {
            txtTip.text = tip;
            showing = true;
        }

        public void Hide()
        {
            showing = false;
        }
        
        private void Update()
        {
            if (showing)
            {
                transform.localPosition = Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            }
            else
            {
                transform.localPosition = Vector3.left * 10000f;
            }
        }
    }
}