using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIFight : MonoBehaviour
    {
        public Image imageProg;
        public GameObject itemRoot;
        public Text txtStage;
        public float timeMax;
        public int spaceSize = 82;
        public int heightOffset = 86;
        public GameObject timeTip;

        private List<UIFightItemCharacter> lstItems;
        private float progMin, progMax;
        
        private void Start()
        {
            progMin = imageProg.rectTransform.localPosition.x - imageProg.rectTransform.sizeDelta.x * 0.5f;
            progMax = progMin + imageProg.rectTransform.sizeDelta.x;
            HideTimeTip();
        }

        public void Init()
        {
            lstItems = new List<UIFightItemCharacter>();
            foreach (var character in GameMgr.Inst.lstCharacters)
            {
                var go = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/ItemCharacter"), itemRoot.transform);
                var uiItem = go.GetComponent<UIFightItemCharacter>();
                uiItem.SetData(character);
                lstItems.Add(uiItem);
            }
        }

        private void Update()
        {
            if (lstItems != null)
            {
                for (int i = 0; i < lstItems.Count; i++)
                {
                    var itemUI = lstItems[i];
                    var normalProg = 1 - itemUI.character.mTimeStiff / timeMax;
                    normalProg = Mathf.Clamp01(normalProg);
                    var localPos =  new Vector3(Mathf.Lerp(progMin, progMax, normalProg), 0f, 0f);
                    int nearCount = 0;
                    for (int j = 0; j < i; j++)
                    {
                        var itemUIOther = lstItems[j];
                        var dis = Mathf.Abs(itemUI.transform.localPosition.x - itemUIOther.transform.localPosition.x);
                        if (dis <= spaceSize) 
                        {
                            nearCount++;
                        }
                    }
                    var offset = heightOffset * nearCount;
                    localPos.y = offset;
                    itemUI.AddPointHeight(offset);
                    itemUI.transform.localPosition = localPos;
                }
            }

            if (GameMgr.Inst.IsInStage(EFightStage.ActionSelect))
            {
                txtStage.text = "主要阶段";
            }
            else if (GameMgr.Inst.IsInStage(EFightStage.ActionReady))
            {
                txtStage.text = "速攻反击阶段";
            }
            else if (GameMgr.Inst.IsInStage(EFightStage.ActionEnd))
            {
                txtStage.text = "结束阶段";
            }
            else
            {
                txtStage.text = "";
            }
        }

        public void ShowTimeTip(float time)
        {
            var normalProg = 1 - time / timeMax;
            normalProg = Mathf.Clamp01(normalProg);
            timeTip.transform.localPosition = new Vector3(Mathf.Lerp(progMin, progMax, normalProg), 0f, 0f);
        }

        public void HideTimeTip()
        {
            timeTip.transform.localPosition = Vector3.left * 10000;
        }
    }
}