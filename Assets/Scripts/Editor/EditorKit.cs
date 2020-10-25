using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorKit : ScriptableObject
{
    [MenuItem("Tools/MyTool/打开Main场景 %m")]
    static void OpenMainScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
    }

    [MenuItem("Tools/MyTool/打开UI场景 %u")]
    static void OpenUIScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/UI.unity");
    }
}