using UnityEngine;
using UnityEditor;

public class EditorSpriteKit : ScriptableObject
{
    [MenuItem("GameObject/规范化精灵描点", false, 1)]
    static void AutoSetSprite()
    {
        var goTarget = Selection.activeGameObject;
        if (goTarget == null)
        {
            return;
        }
        var spriteRender = goTarget.GetComponent<SpriteRenderer>();
        if (spriteRender == null)
        {
            return;
        }
        var sprite = spriteRender.sprite;

        Vector2 worldSize = spriteRender.bounds.size;

        Vector2 wPos = goTarget.transform.position;
        var pivot = Vector2.one * 0.5f - wPos / worldSize;

        var assetPath = AssetDatabase.GetAssetPath(sprite.texture);

        TextureImporter txuIm = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        TextureImporterSettings txuImSetting = new TextureImporterSettings();
        txuIm.ReadTextureSettings(txuImSetting);
        txuImSetting.spriteAlignment = 9;
        txuIm.SetTextureSettings(txuImSetting);
        txuIm.spritePivot = pivot;
        txuIm.SaveAndReimport();
        goTarget.transform.localPosition = Vector3.zero;
    }
}