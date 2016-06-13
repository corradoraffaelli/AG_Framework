using UnityEngine;
using System.Collections;

namespace AG_Framework
{
    public class AG_Node : ScriptableObject
    {

        public string nodeName;
        public Rect nodeRect;
        public AG_Graph parentGraph;
        public AG_NodeType nodeType;
        public Vector2 nodeSize;

        public void InitNode()
        {

        }

		public AG_Node CreateNode(Vector2 mousePosition, AG_Graph parentGraph)
		{
			AG_Node currentNode = null;

//			NodeBase currentNode = null;
//			switch (nodeType)
//			{
//			case NodeType.Float:
//				currentNode = ScriptableObject.CreateInstance<FloatNode>();
//				currentNode.NodeName = "Float Node";
//				break;
//			case NodeType.Add:
//				currentNode = ScriptableObject.CreateInstance<AddNode>();
//				currentNode.NodeName = "Add Node";
//				break;
//			}

			currentNode = CreateNodeInstance ();

			if (currentNode != null)
			{
				currentNode.InitNode();

				var nodeRect = currentNode.nodeRect;

				nodeRect.x = mousePosition.x;
				nodeRect.y = mousePosition.y;

				currentNode.nodeRect = nodeRect;
				currentNode.parentGraph = parentGraph;


			}

			return currentNode;
		}

		public static AG_Node CreateNodeInstance()
		{
			Debug.LogError("Rememeber to override");
			return null;
		}

//		public static void DeleteNode(NodeBase currentNode, NodeGraph currentGraph)
//		{
//			if (currentGraph != null)
//			{
//				currentGraph.Nodes.Remove(currentNode);
//				GameObject.DestroyImmediate(currentNode, true);
//				AssetDatabase.SaveAssets();
//				AssetDatabase.Refresh();
//			}
//		}
    }
}


