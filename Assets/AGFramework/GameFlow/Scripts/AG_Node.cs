using UnityEngine;
using System.Collections;
//using UnityEditor;
using System;
using System.Collections.Generic;

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

        //public bool hasInputEvent = true;
        //public bool hasOutputEvent = true;

        //[Serializable]
        //public class LinkNode
        //{
        //	public bool input;
        //	public bool isOccupied;
        //	public string name;
        //	public AG_Node otherNode;

        //}

        //public List<LinkNode> inputNodes;
        //public List<LinkNode> outputNodes;

        //public Rect inputLinkRect;
        //public Rect outputLinkRect;

        public List<AG_NodeLinkPoint> inputPoints;
        public List<AG_NodeLinkPoint> outputPoints; 

        public virtual void InitNode()
        {

        }

        public void AddInputPoint(string pointName, bool showName, int maxLinks)
        {
            if (inputPoints == null)
                inputPoints = new List<AG_NodeLinkPoint>();

            AG_NodeLinkPoint pointToAdd = new AG_NodeLinkPoint(pointName, showName, PointType.Input, maxLinks);

            inputPoints.Add(pointToAdd);
        }

        public void AddOutputPoint(string pointName, bool showName, int maxLinks)
        {
            if (outputPoints == null)
                outputPoints = new List<AG_NodeLinkPoint>();

            AG_NodeLinkPoint pointToAdd = new AG_NodeLinkPoint(pointName, showName, PointType.Output, maxLinks);

            outputPoints.Add(pointToAdd);
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
			case AG_NodeType.Begin:
				currentNode = ScriptableObject.CreateInstance<AG_BeginNode>();
				currentNode.nodeName = "Starting Node";
				break;
            case AG_NodeType.Test:
                currentNode = ScriptableObject.CreateInstance<AG_TestNode>();
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

    [Serializable]
    public class AG_NodeLink
    {
        // The input node of the link
        public AG_Node inputNode;
        // The input point of the link, the point is one of the output points of the inputNode
        public AG_NodeLinkPoint beginningPoint;

        // The output node of the link
        public AG_Node outputNode;
        // The output point of the link, the point is one of the input points of the outputNode
        public AG_NodeLinkPoint endingPoint;

        // The duration of the transition
        int duration;

        public AG_NodeLink(AG_Node _inputNode, AG_NodeLinkPoint _beginningPoint, AG_Node _outputNode, AG_NodeLinkPoint _endingPoint)
        {
            inputNode = _inputNode;
            beginningPoint = _beginningPoint;
            outputNode = _outputNode;
            endingPoint = _endingPoint;
        }

        public AG_NodeLink(AG_Node _inputNode, AG_NodeLinkPoint _beginningPoint, AG_Node _outputNode, AG_NodeLinkPoint _endingPoint, int _duration)
        {
            inputNode = _inputNode;
            beginningPoint = _beginningPoint;
            outputNode = _outputNode;
            endingPoint = _endingPoint;
            duration = _duration;
        }



    }

    [Serializable]
    public class AG_NodeLinkPoint
    {
        // The name to show in the GUI
        public string name;
        public bool showName;

        // Link type, input or output
        public PointType pointType;

        // Links attached to the point
        public List<AG_NodeLink> links;
        // Max number of links attached to the point
        public int maxLinks = 200;

        // Punctual position of the point, used for draw lines in inspector
        //public Vector2 position;
        public Rect imageRect;
        // Rect that can be clicked to draw links
        public Rect clickableRect;
        // If a link is being drawn and the mouse is inside the Rect, the link is snapped
        public Rect snappableRect;

        public AG_NodeLinkPoint(string _name, bool _showName, PointType _pointType, int _maxLinks)
        {
            name = _name;
            showName = _showName;
            pointType = _pointType;
            maxLinks = _maxLinks;
        }

        public void AddLink(AG_NodeLink linkToAdd)
        {
            if (links == null)
                links = new List<AG_NodeLink>();

            links.Add(linkToAdd);
        }

        public void RemoveLink(AG_NodeLink linkToRemove)
        {
            if (links != null)
            {
                links.Remove(linkToRemove);
            }
        }
    }

    public enum PointType
    {
        Input,
        Output
    }
}


