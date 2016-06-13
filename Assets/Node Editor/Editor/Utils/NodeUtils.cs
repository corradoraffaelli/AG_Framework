using UnityEngine;
using System.Collections;
using UnityEditor;

namespace NodeBasedEditor
{
    public static class NodeUtils
    {
        public static void CreateNodeGraph(string graphName)
        {
            var nodeGraph = ScriptableObject.CreateInstance<NodeGraph>();
            if (nodeGraph != null)
            {
                nodeGraph.GraphName = graphName;
                nodeGraph.InitGraph();

                AssetDatabase.CreateAsset(nodeGraph, "Assets/Node Editor/Database/" + graphName + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                var currentWorkView = NodeEditorWindow.CurrentWindow = EditorWindow.GetWindow<NodeEditorWindow>();
                if (currentWorkView != null)
                {
                    currentWorkView.CurrentGraph = nodeGraph;
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Node Message:", "Unable to create graph", "OK");
            }
        }

        public static void LoadGraph()
        {
            NodeGraph nodeGraph = null;

            string graphPath = EditorUtility.OpenFilePanel("Load Graph", Application.dataPath + "/Node Editor/Database/",
                "asset");

            int appPathLength = Application.dataPath.Length;

            string finalPath = graphPath.Substring(appPathLength - 6);

            nodeGraph = (NodeGraph)AssetDatabase.LoadAssetAtPath(finalPath, typeof(NodeGraph));

            if (nodeGraph == null)
            {
                EditorUtility.DisplayDialog("Node Message:", "Unable to load the graph", "OK");
            }

            var currentNodeEditorWindow = EditorWindow.GetWindow<NodeEditorWindow>();
            currentNodeEditorWindow.CurrentGraph = nodeGraph;
        }

        public static void UnloadGraph()
        {
            var currentWorkView = NodeEditorWindow.CurrentWindow = EditorWindow.GetWindow<NodeEditorWindow>();
            if (currentWorkView != null)
            {
                currentWorkView.CurrentGraph = null;
            }
        }

        public static void CreateNode(NodeGraph currentGraph, NodeType nodeType, Vector2 mousePosition)
        {
            if (currentGraph == null) return;

            NodeBase currentNode = null;
            switch (nodeType)
            {
                case NodeType.Float:
                    currentNode = ScriptableObject.CreateInstance<FloatNode>();
                    currentNode.NodeName = "Float Node";
                    break;
                case NodeType.Add:
                    currentNode = ScriptableObject.CreateInstance<AddNode>();
                    currentNode.NodeName = "Add Node";
                    break;
            }

            if (currentNode != null)
            {
                currentNode.InitNode();

                var nodeRect = currentNode.NodeRect;

                nodeRect.x = mousePosition.x;
                nodeRect.y = mousePosition.y;

                currentNode.NodeRect = nodeRect;
                currentNode.ParentGraph = currentGraph;
                currentGraph.Nodes.Add(currentNode);

                AssetDatabase.AddObjectToAsset(currentNode, currentGraph);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        public static void DeleteNode(NodeBase currentNode, NodeGraph currentGraph)
        {
            if (currentGraph != null)
            {
                currentGraph.Nodes.Remove(currentNode);
                GameObject.DestroyImmediate(currentNode, true);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        public static void DrawGrid(Rect viewRect, float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(viewRect.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(viewRect.height / gridSpacing);

            Handles.BeginGUI();

            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            for (int x = 0; x < widthDivs; x++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * x, 0f, 0f), new Vector3(gridSpacing * x, viewRect.height, 0f));
            }

            for (int y = 0; y < heightDivs; y++)
            {
                Handles.DrawLine(new Vector3(0f, gridSpacing * y, 0f), new Vector3(viewRect.width, gridSpacing * y, 0f));
            }

            Handles.EndGUI();
        }
    }
}
