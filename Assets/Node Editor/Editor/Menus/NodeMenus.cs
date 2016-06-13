using UnityEngine;
using System.Collections;
using UnityEditor;

namespace NodeBasedEditor
{
    public class NodeMenus
    {
        [MenuItem("Node Editor/Launch Editor")]
        public static void InitNodeEditor()
        {
            NodeEditorWindow.InitEditorWindow();
        }
    }
}
