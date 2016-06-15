﻿using UnityEngine;
using System.Collections;
//using UnityEditor;
using System;

namespace AG_Framework
{
    public class AG_Node : ScriptableObject
    {

        public string nodeName;
        public Rect nodeRect;
        public AG_Graph parentGraph;
        public AG_NodeType nodeType;
        public Vector2 nodeSize;

		public bool isSelected;

		[Serializable]
		public class NodeInput
		{
			public bool isOccupied;
			public AG_Node inputNode;
		}

		[Serializable]
		public class NodeOutput
		{
			public bool isOccupied;
			public AG_Node outputNode;
		}

        public virtual void InitNode()
        {

        }

		public static AG_Node CreateNode(Vector2 mousePosition, AG_Graph parentGraph, AG_NodeType nodeType)
		{
			AG_Node currentNode = null;

			switch (nodeType)
			{
			case AG_NodeType.Dialogue:
				currentNode = ScriptableObject.CreateInstance<AG_DialogueNode>();
				currentNode.nodeName = "Dialogue Node";
				break;
//			case AG_NodeType.Add:
//				currentNode = ScriptableObject.CreateInstance<AddNode>();
//				currentNode.NodeName = "Add Node";
//				break;
			}


			if (currentNode != null)
			{
				currentNode.InitNode();

				Rect nodeRect = new Rect (mousePosition.x - currentNode.nodeSize.x/2, mousePosition.y, currentNode.nodeSize.x, currentNode.nodeSize.y);

				currentNode.nodeRect = nodeRect;
				currentNode.parentGraph = parentGraph;
			}

			return currentNode;
		}

		public void MouseDragged(Event e)
		{
			if (isSelected)
			{
				nodeRect.x += e.delta.x;
				nodeRect.y += e.delta.y;
			}
		}

		public void LeftClick(Event e)
		{
			//I select the node for moving only if the click is in the upper part

			Rect overPosition = new Rect (nodeRect.x, nodeRect.y, nodeSize.x, 30.0f);

			if (overPosition.Contains (e.mousePosition))
				isSelected = true;
		}



    }
}


