using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIWorldInfo : UIBase
{
  public Transform tfGridChraracters;
  public GameObject pfbUIItemChracter;
  public Text txtNumOfFood;
  public Text txtNumOfGold;
  public Text txtLayer;

  List<UIItemCharacterForWorldInfo> lstUIItemCharacters;

    public override void Init()
    {
        base.Init();
        lstUIItemCharacters = new List<UIItemCharacterForWorldInfo>();
    }

  

  public override void OnShow()
  {
      base.OnShow();
      //角色列表
      for (int i = 0; i < WorldRaidData.Inst.lstCharacters.Count; i++)
      {
          var data = WorldRaidData.Inst.lstCharacters[i];
          var uiItem = UIItemBase.Create<UIItemCharacterForWorldInfo>(tfGridChraracters.transform, pfbUIItemChracter);
          uiItem.Set(data);
          uiItem.Refresh();
      }
      //食物
      txtNumOfFood.text = "x" + WorldRaidData.Inst.numOfFood;
      txtNumOfGold.text = "x" + WorldRaidData.Inst.numOfGold;
      txtLayer.text = $"{WorldRaidData.Inst.layer}/{WorldRaidData.Inst.maxLayer}层";
  }

    public override void OnHide()
    {
        base.OnHide();
        foreach (var item in lstUIItemCharacters)
        {
            item.Cache();
        }
    }
}
