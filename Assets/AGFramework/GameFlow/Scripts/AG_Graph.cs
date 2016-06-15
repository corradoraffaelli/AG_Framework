using UnityEngine;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

using System;
using System.Collections;
using System.Collections.Generic;

namespace AG_Framework
{
    [Serializable]
    public class AG_Graph : ScriptableObject
    {
        public string graphName = "New chapter";

        public List<AG_Node> nodes;

        private void OnEnable()
        {
            if (nodes == null)
            {
                nodes = new List<AG_Node>();
            }
        }

        public void InitGraph()
        {
            if (nodes.Count > 0)
            {
                foreach (var node in nodes)
                {
                    node.InitNode();
                }
            }
        }

		#region STATIC METHODS

		public static AG_Graph CreateNodeGraph(string graphName)
		{
			var nodeGraph = ScriptableObject.CreateInstance<AG_Graph>();
			if (nodeGraph != null)
			{
				nodeGraph.graphName = graphName;
				nodeGraph.InitGraph();
			}

			return nodeGraph;
		}


//		public static void UnloadGraph()
//		{
//			var currentWorkView = NodeEditorWindow.CurrentWindow = EditorWindow.GetWindow<NodeEditorWindow>();
//			if (currentWorkView != null)
//			{
//				currentWorkView.CurrentGraph = null;
//			}
//		}

		#endregion STATIC METHODS

    }
}


