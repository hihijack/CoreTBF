using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.Editor
{
    public class EditorWinTable : EditorWindow
    {
        private Object target;
        private UnityEditor.Editor targetEditor;

        static List<EditorWinTable> lst = new List<EditorWinTable>();

        [MenuItem("Assets/在表格窗口打开")]
        static void AddWindow()
        {
            //GetWindow<EditorWinTable>(false, "表格窗口").SetTarget(Selection.activeObject);
            EditorWinTable existWin = null;
            foreach (var win in lst)
            {
                if (win.target == Selection.activeObject)
                {
                    existWin = win;
                    break;
                }
            }

            if (existWin == null)
            {
                //打开多个窗口
                EditorWinTable inst = ScriptableObject.CreateInstance<EditorWinTable>();
                inst.Show();
                inst.SetTarget(Selection.activeObject);
                lst.Add(inst);
            }
        }

        private void OnDestroy()
        {
            lst.Remove(this);
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }
            EditorGUI.BeginChangeCheck();
            target = EditorGUILayout.ObjectField("目标:" , target, typeof(ScriptableObject), false);
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
            this.titleContent =  new GUIContent(target.name);
        }
    }
}