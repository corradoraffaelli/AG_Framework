using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;

namespace NodeBasedEditor
{
    [Serializable]
    public class NodeGraph : ScriptableObject
    {
        public string GraphName = "New graph";
        public NodeBase SelectedNode { get; set; }
        public List<NodeBase> Nodes;
        public bool ShowProperties { get; set; }

        public bool WantsConnection { get; set; }
        public NodeBase ConnectionNode { get; set; }

        private void OnEnable()
        {
            if (Nodes == null)
            {
                Nodes = new List<NodeBase>();
            }
        }

        public void InitGraph()
        {
            if (Nodes.Count > 0)
            {
                foreach (var node in Nodes)
                {
                    node.InitNode();
                }
            }
        }

        public void UpdateGraph()
        {

        }

#if UNITY_EDITOR

        public void UpdateGraphGUI(Event e, Rect viewRect, GUISkin guiSkin)
        {
            if (Nodes.Count > 0)
            {
                ProcessEvents(e, viewRect);
                foreach (var node in Nodes)
                {
                    node.UpdateNodeGUI(e, viewRect, guiSkin);
                }
            }

            if (WantsConnection && ConnectionNode != null)
            {
                DrawConnectionToMouse(e.mousePosition);
            }

            if (e.type == EventType.Layout && SelectedNode != null)
            {
                ShowProperties = true;
            }

            EditorUtility.SetDirty(this);
        }

#endif

        private void ProcessEvents(Event e, Rect viewRect)
        {
            if (viewRect.Contains(e.mousePosition))
            {
                if (e.button == 0 && e.type == EventType.MouseDown)
                {
                    if (!WantsConnection)
                    {
                        DeselectAllNodes();
                    }

                    foreach (var node in Nodes)
                    {
                        if (node.NodeRect.Contains(e.mousePosition))
                        {
                            SelectedNode = node;
                            SelectedNode.IsSelected = true;
                            break;
                        }
                    }
                }
            }
        }

        private void DeselectAllNodes()
        {
            Nodes.ForEach(x => x.IsSelected = false);
            ShowProperties = false;
            SelectedNode = null;
            WantsConnection = false;
            ConnectionNode = null;
        }

        private void DrawConnectionToMouse(Vector2 mousePosition)
        {
            Handles.BeginGUI();

            Handles.color = Color.white;
            Handles.DrawLine(
                new Vector2(ConnectionNode.NodeRect.x + ConnectionNode.NodeRect.width + 24f,
                    ConnectionNode.NodeRect.y + ConnectionNode.NodeRect.height * 0.5f ), mousePosition);

            Handles.EndGUI();
        }

    }
}
