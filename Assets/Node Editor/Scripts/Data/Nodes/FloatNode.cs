using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;

namespace NodeBasedEditor
{
    public class FloatNode : NodeBase
    {
        public float NodeValue;
        public NodeOutput Output;

        public FloatNode()
        {
            Output = new NodeOutput();
        }

        public override void InitNode()
        {
            base.InitNode();
            NodeType = NodeType.Float;
            NodeRect = new Rect(10f, 10f, 150f, 65f);
        }

        public override void UpdateNode(Event e, Rect viewRect)
        {
            base.UpdateNode(e, viewRect);
        }

#if UNITY_EDITOR

        public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin guiSkin)
        {
            base.UpdateNodeGUI(e, viewRect, guiSkin);

            GUILayout.Space(40f);

            if (GUI.Button(new Rect(NodeRect.x + NodeRect.width, NodeRect.y + NodeRect.height * 0.5f - 12f, 24f, 24f), "", guiSkin.GetStyle("NodeOutput")))
            {
                if (ParentGraph != null)
                {
                    ParentGraph.WantsConnection = true;
                    ParentGraph.ConnectionNode = this;
                }
            }

        }

        public override void DrawNodeProperties(Rect viewRect)
        {
            base.DrawNodeProperties(viewRect);

            NodeValue = EditorGUILayout.FloatField("Float value", NodeValue);
        }

#endif
    }
}
