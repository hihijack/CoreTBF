using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Editor
{
    public class UGUIEditorHelper : ScriptableObject
    {
        [MenuItem("GameObject/UIUnit/AddImage &#I")]
        static void AddImage()
        {
            var go = new GameObject("Image", typeof(Image));
            go.transform.parent = Selection.activeTransform;
            go.transform.localPosition = Vector3.zero;
            Selection.activeGameObject = go;
        }
        
        [MenuItem("GameObject/UIUnit/AddText &#T")]
        static void AddText()
        {
            var go = new GameObject("Text", typeof(Text));
            go.transform.parent = Selection.activeTransform;
            go.transform.localPosition = Vector3.zero;
            Selection.activeGameObject = go;
        }
    }
}