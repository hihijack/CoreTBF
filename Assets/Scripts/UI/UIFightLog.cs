using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class UIFightLog : MonoBehaviour
{
    public Text txtContet;

    StringBuilder sbLog;

    private void Awake()
    {
        sbLog = new StringBuilder();
    }


    public void AppendLog(string log)
    {
        sbLog.AppendLine(log);
        txtContet.text = sbLog.ToString();
    }
}
