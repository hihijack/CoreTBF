using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIItemMP : MonoBehaviour
{
    public Image imgFill;
    Slider _sld;

    Color _cActive;
    Color _cUnActive;

    private void Awake()
    {
        _sld = GetComponent<Slider>();
        _cActive = new Color(0.2f, 0.9f, 1f);
        _cUnActive = new Color(0.2f, 0.4f, 0.4f);
    }

    public void SetVal(float prog)
    {
        _sld.value = prog;
        if (prog >= 1f)
        {
            imgFill.color = _cActive;
        }
        else
        {
            imgFill.color = _cUnActive;
        }
    }
   
}
