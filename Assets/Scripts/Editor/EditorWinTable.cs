using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.Editor
{
    public class EditorWinTable : EditorWindow
    {
        private Object target;
        private UnityEditor.Editor targetEditor;
    
        [MenuItem("Assets/在表格窗口打开")]
        static void AddWindow()
        {
            //GetWindow<EditorWinTable>(false, "表格窗口").SetTarget(Selection.activeObject);
            //打开多个窗口
            EditorWinTable inst = ScriptableObject.CreateInstance<EditorWinTable>();
            inst.Show();
            inst.SetTarget(Selection.activeObject);
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            EditorGUI.BeginChangeCheck();
            target = EditorGUILayout.ObjectField("目标:" , target, typeof(ScriptableObject));
            if (EditorGUI.EndChangeCheck())
            {
                //使用目标创建一个Editor
                targetEditor = UnityEditor.Editor.CreateEditor(target);
            }

            //绘制Editor
            if (targetEditor != null)
            {
                targetEditor.OnInspectorGUI();
            }
        }

        public void SetTarget(Object target)
        {
            this.target = target;
            targetEditor = UnityEditor.Editor.CreateEditor(target);
            this.title = target.name;
        }
    }
}