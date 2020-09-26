using UnityEngine;
using System.Collections;

public class TestLoadSprite : MonoBehaviour
{
    
    // Use this for initialization
    void Start()
    {
        var spRender = GetComponent<SpriteRenderer>();
        var sp = Resources.Load<Sprite>("Sprites/Hakumen/atk");
        spRender.sprite = sp;
    }
}
