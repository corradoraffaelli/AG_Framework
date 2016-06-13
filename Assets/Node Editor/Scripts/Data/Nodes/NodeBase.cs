using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace NodeBasedEditor
{
    public class NodeBase : ScriptableObject
    {
        public string NodeName;
        public Rect NodeRect;
        public NodeGraph ParentGraph;
        public NodeType NodeType;
        public bool IsSelected { get; set; }
        public GUISkin NodeSkin { get; set; }

        [Serializable]
        public class NodeInput
        {
            public bool isOccupied;
            public NodeBase inputNode;
        }

        [Serializable]
        public class NodeOutput
        {
            public bool isOccupied;
            public NodeBase outputNode;
        }

       

        public virtual void InitNode()
        {

        }

        public virtual void UpdateNode(Event e, Rect viewRect)
        {
            ProcessEvents(e, viewRect);
        }

#if UNITY_EDITOR
        public virtual void UpdateNodeGUI(Event e, Rect viewRect, GUISkin guiSkin)
        {
            ProcessEvents(e, NodeRect);

            string currentStyle = IsSelected ? "NodeSelected" : "NodeDefault";
            GUI.Box(NodeRect, NodeName, guiSkin.GetStyle(currentStyle));

            EditorUtility.SetDirty(this);
        }

        public virtual void DrawNodeProperties(Rect viewRect)
        {

        }
#endif

        private void ProcessEvents(Event e, Rect viewRect)
        {
            if (IsSelected)
            {
                if (e.type == EventType.MouseDrag)
                {
                    var rect = NodeRect;

                    rect.x += e.delta.x;
                    rect.y += e.delta.y;

                    NodeRect = rect;
                }
            }
        }
    }
}
