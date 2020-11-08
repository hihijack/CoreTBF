using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class UIFightLog : UIBase
{
    public Text txtContet;
    public Scrollbar scrollBar;

    StringBuilder sbLog;

    public static UIFightLog Inst{get; private set;}

    protected override void OnAwake()
    {
        base.OnAwake();
        Inst = this;
        sbLog = new StringBuilder();
    }

    public override void OnHide()
    {
        base.OnHide();
        sbLog.Clear();
        txtContet.text = "";
    }

    public void AppendLog(string log)
    {
        sbLog.AppendLine(log);
        txtContet.text = sbLog.ToString();
        scrollBar.value = 0;
    }
}
