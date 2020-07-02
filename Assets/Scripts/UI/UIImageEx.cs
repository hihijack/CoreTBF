using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIImageEx : MonoBehaviour
{
    Image _img;

    private void Awake()
    {
        _img = GetComponent<Image>();
        _img.material = new Material(_img.material);
    }

    public void SetOutLineEnable(bool enable)
    {
        if (enable)
        {
            _img.material.SetFloat("_OutlineWidth", 0.041f);
        }
        else
        {
            _img.material.SetFloat("_OutlineWidth", 0f);
        }
    }
}
