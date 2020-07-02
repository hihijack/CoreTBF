using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class UIFightLog : MonoBehaviour
{
    public Text txtContet;
    public Scrollbar scrollBar;

    StringBuilder sbLog;



    private void Awake()
    {
        sbLog = new StringBuilder();
    }


    public void AppendLog(string log)
    {
        sbLog.AppendLine(log);
        txtContet.text = sbLog.ToString();
        scrollBar.value = 0;
    }
}
