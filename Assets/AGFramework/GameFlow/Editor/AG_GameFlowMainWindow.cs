using UnityEngine;
using System.Collections;
using System.ComponentModel;
using UnityEditor;
using subjectnerd;
using System.Collections.Generic;

namespace AG_Framework
{
    public class AG_GameFlowMainWindow : EditorWindow
    {
        #region Variables

        public static AG_GameFlowMainWindow currentWindow;

        GUISkin viewSkin;
        public AG_Graph currentGraph;
		public AG_Node selectedNode;
		public AG_Node rightClickNode;
        public string graphTitle;

        Rect windowRect;

		Vector2 mousePosition;

		AG_Node clickedInputNode;
		AG_Node clickedOutputNode;
//		AG_Node.NodeOutput clickedOutputNode;

		bool draggingLinkNode = false;

		private Vector2 scale = new Vector2(1, 1);
		private Vector2 pivotPoint = Vector2.zero;
		float zoomScale = 1.0f;

        #endregion

        #region Main Methods

        public static void InitEditorWindow()
        {
            currentWindow = EditorWindow.GetWindow<AG_GameFlowMainWindow>();
            currentWindow.titleContent = new GUIContent("Game Flow");
            
        }

        private void OnEnable()
        {
        }

        private void OnDestroy()
        {
        }

        private void Update()
        {
        }

        private void OnGUI()
        {
            if (currentWindow == null)
                currentWindow = EditorWindow.GetWindow<AG_GameFlowMainWindow>();




			GUIUtility.ScaleAroundPivot(scale, pivotPoint);


            // Skin loading
            //if (viewSkin == null)
            //    viewSkin = Resources.Load<GUISkin>("GUI Skins/Editor Skins/Node Editor Skin");

            windowRect = new Rect(0, 0, position.width, position.height);

            // Title
            string graphTitle = "";
            if (currentGraph == null)
            {
                graphTitle = "No Chapter loaded";
            }
            else
            {
                graphTitle = currentGraph.graphName;
            }
            //GUI.Box(position, graphTitle, ViewSkin.GetStyle("ViewBG"));
            GUI.Box(new Rect(0,0,position.width, position.height), graphTitle);
           
            

			NodesGUI ();









//			subjectnerd.GLDraw.DrawConnectingCurve (new Vector2 (10.0f, 30.0f), new Vector2 (500.0f, 300.0f), Color.red, 1.5f);

			//Bottom Area (Chapter choose)
			Rect bottomRect = new Rect(10, position.height - 30.0f, 200, 30.0f);
			GUILayout.BeginArea(bottomRect);

			if (GUILayout.Button("Press me"))
			    Debug.Log("Button pressed");

			GUILayout.EndArea();

            // Get and process the current event
            Event e = Event.current;
            ProcessEvents(e);

			if (draggingLinkNode) {
				if (clickedInputNode != null)
					subjectnerd.GLDraw.DrawConnectingCurve (clickedInputNode.inputLinkRect.center, e.mousePosition, Color.red, 1.2f);
				if (clickedOutputNode != null)
					subjectnerd.GLDraw.DrawConnectingCurve (e.mousePosition, clickedOutputNode.outputLinkRect.center, Color.red, 1.2f);
			}



			if (e.type == EventType.ScrollWheel)
			{
				var zoomDelta = 0.1f;
				zoomDelta = e.delta.y < 0 ? zoomDelta : -zoomDelta;
				zoomScale += zoomDelta;
				zoomScale = Mathf.Clamp(zoomScale, 0.25f, 1.25f);

				scale = new Vector2 (zoomScale, zoomScale);
				pivotPoint = e.mousePosition;

				e.Use();


			}



			//Scale
//			pivotPoint = e.mousePosition;




            Repaint();
        }

        private void ProcessEvents(Event e)
        {

			if (currentGraph != null && currentGraph.nodes != null) {
				foreach (AG_Node node in currentGraph.nodes) {
					if (node.inputLinkRect.Contains (e.mousePosition) && e.type == EventType.MouseDown && e.button == 0) {

						draggingLinkNode = true;
						clickedInputNode = node;

						return;
					}
					else if (node.outputLinkRect.Contains (e.mousePosition) && e.type == EventType.MouseDown && e.button == 0) {

						draggingLinkNode = true;
						clickedOutputNode = node;

						return;
					}
				}
			}

            if (currentGraph != null && currentGraph.nodes != null)
            {
                foreach(AG_Node node in currentGraph.nodes)
                {
                    if (node.nodeRect.Contains(e.mousePosition))
                    {
                        if (e.type == EventType.mouseDown)
                        {
                            if (e.button == 0)
                            {
								ProcessNodeLeftClick (e, node);
								selectedNode = node;
                            } else if (e.button == 1)
                            {
								ProcessNodeRightClick (e, node);
                            }

                            return;
                        }

                    }
                }
            }

            if (windowRect.Contains(e.mousePosition))
            {
                if (e.type == EventType.MouseDown)
                {
                    if (e.button == 0)
                    {
                        Debug.Log("Left click inside window");
                    }

                    if (e.button == 1)
                    {
						ProcessContextMenuWindow (e);
                    }
                }
            }

			//If I'm dragging the mouse (left button) I move the selectedNode
			if (e.type == EventType.MouseDrag)
			{
				if (e.button == 0)
				{
					selectedNode.MouseDragged (e);
				} 

				return;
			}

			// If the left mouse button is left, I deselect everything
			if (e.button == 0 && e.type == EventType.mouseUp) {
				DeselectAllNodes ();

				if (draggingLinkNode) {
					if (currentGraph != null && currentGraph.nodes != null) {
						foreach (AG_Node node in currentGraph.nodes) {
							if (clickedInputNode != null) {
								if (node.outputLinkRect.Contains (e.mousePosition) && e.type == EventType.MouseUp && e.button == 0) {

									AG_Node.LinkNode linkNode = new AG_Node.LinkNode ();
									linkNode.otherNode = clickedInputNode;
									linkNode.input = false;

									if (node.outputNodes == null)
										node.outputNodes = new List<AG_Node.LinkNode> ();
									node.outputNodes.Add (linkNode);

									AG_Node.LinkNode inputLinkNode = new AG_Node.LinkNode ();
									inputLinkNode.otherNode = node;
									inputLinkNode.input = true;

									if (clickedInputNode.inputNodes == null)
										clickedInputNode.inputNodes = new List<AG_Node.LinkNode> ();
									clickedInputNode.inputNodes.Add (inputLinkNode);

//									return;
								}
							}

							if (clickedOutputNode != null) {
								if (node.inputLinkRect.Contains (e.mousePosition) && e.type == EventType.MouseUp && e.button == 0) {

									AG_Node.LinkNode linkNode = new AG_Node.LinkNode ();
									linkNode.otherNode = clickedOutputNode;
									linkNode.input = true;

									if (node.inputNodes == null)
										node.inputNodes = new List<AG_Node.LinkNode> ();
									node.inputNodes.Add (linkNode);

									AG_Node.LinkNode outputLinkNode = new AG_Node.LinkNode ();
									outputLinkNode.otherNode = node;
									outputLinkNode.input = false;

									if (clickedOutputNode.outputNodes == null)
										clickedOutputNode.outputNodes = new List<AG_Node.LinkNode> ();
									clickedOutputNode.outputNodes.Add (outputLinkNode);

//									return;
								}
							}

//							if (node.inputLinkRect.Contains (e.mousePosition) && e.type == EventType.MouseDown && e.button == 0) {
//
//								draggingLinkNode = true;
//								clickedInputNode = node;
//
//								return;
//							}
//							else if (node.outputLinkRect.Contains (e.mousePosition) && e.type == EventType.MouseDown && e.button == 0) {
//
//								draggingLinkNode = true;
//								clickedOutputNode = node;
//
//								return;
//							}
						}
					}
				}

				draggingLinkNode = false;
				clickedInputNode = null;
				clickedOutputNode = null;
			}
				

        }

        #endregion

		void NodesGUI()
		{
			if (currentGraph != null && currentGraph.nodes != null) 
			{
				foreach (AG_Node node in currentGraph.nodes)
				{
					GUI.Box (node.nodeRect, node.nodeName);

					if (node.hasInputEvent) {
						node.inputLinkRect = new Rect (node.nodeRect.x - 6.0f,
							node.nodeRect.y + node.nodeRect.height - 20.0f,
							10.0f, 10.0f);
					}

					if (node.hasOutputEvent) {
						node.outputLinkRect = new Rect (node.nodeRect.x + node.nodeRect.width - 4.0f,
							node.nodeRect.y + node.nodeRect.height - 20.0f,
							10.0f, 10.0f);
					}



					Texture2D image = Resources.Load ("SmallCircle", typeof(Texture2D)) as Texture2D;
					if (node.hasInputEvent)
						GUI.DrawTexture (node.inputLinkRect, image);
					if (node.hasOutputEvent)
						GUI.DrawTexture (node.outputLinkRect, image);

					LinesGUI (node);
				}
			}

		}

		void LinesGUI(AG_Node node)
		{
			if (node.inputNodes != null && node.inputNodes.Count >= 0) {
				foreach (AG_Node.LinkNode inputNode in node.inputNodes) {
					if (inputNode.otherNode != null) {
						GLDraw.DrawConnectingCurve (inputNode.otherNode.outputLinkRect.center, node.inputLinkRect.center, Color.blue, 1.3f);
					}
				}
			}

		}

		private void ProcessContextMenuWindow(Event e)
		{
			GenericMenu menu = new GenericMenu();

			mousePosition = e.mousePosition;

			menu.AddItem(new GUIContent("Create graph"), false, ContextCallback, "0");
			menu.AddItem(new GUIContent("Load graph"), false, ContextCallback, "1");

			if (currentGraph != null)
			{
//				menu.AddSeparator("");
//				menu.AddItem(new GUIContent("Unload graph"), false, ContextCallback, "2");

				menu.AddSeparator("");
				menu.AddItem(new GUIContent("Begin Node"), false, ContextCallback, "2");
				menu.AddItem(new GUIContent("Dialogue Node"), false, ContextCallback, "3");

//				menu.AddItem(new GUIContent("Add Node"), false, ContextCallback, "4");

			}


			menu.ShowAsContext();
			e.Use();
		}

		private void ContextCallback(object obj)
		{
			switch (obj.ToString ()) {
			case "0":
				AG_CreateGraphWindow.InitNodePopup ();
				break;
			case "1":
				LoadGraph ();
				break;
			case "2":
				
				AG_Node beginNode = AG_Node.CreateNode (mousePosition, currentGraph, AG_NodeType.Begin);

				AssetDatabase.AddObjectToAsset (beginNode, currentGraph);
				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh ();

				currentGraph.nodes.Add (beginNode);

				break;


			
			case "3":
				
				AG_Node dialogueNode = AG_Node.CreateNode (mousePosition, currentGraph, AG_NodeType.Dialogue);

				AssetDatabase.AddObjectToAsset (dialogueNode, currentGraph);
				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh ();

				currentGraph.nodes.Add (dialogueNode);

				break;
				

			}
		}

        #region Utility Methods

		void DeselectAllNodes()
		{
			if (currentGraph != null) {
				foreach (AG_Node node in currentGraph.nodes) {
					node.isSelected = false;
				}
			}
		}

		void ProcessNodeLeftClick(Event e, AG_Node node)
		{
			node.LeftClick (e);
		}

		void ProcessNodeRightClick(Event e, AG_Node node)
		{
			ProcessContextMenuNode (e, node);
		}

		private void ProcessContextMenuNode(Event e, AG_Node node)
		{
			GenericMenu menu = new GenericMenu();

			mousePosition = e.mousePosition;

			rightClickNode = node;

			menu.AddItem(new GUIContent("Delete node"), false, ContextCallbackNode, "0");

			menu.ShowAsContext();
			e.Use();
		}

		private void ContextCallbackNode(object obj)
		{
			switch (obj.ToString ()) {
			case "0":
				DeleteNode (rightClickNode, currentGraph);
				break;
			}
		}


		public static void LoadGraph()
		{
			AG_Graph nodeGraph = null;

			int appPathLength = Application.dataPath.Length;
			string dataPathWithoutAssets = Application.dataPath.Substring (0, appPathLength - 6);

//			Debug.Log (dataPathWithoutAssets + ConstantKeys.DataPath_GameFlow);

			string graphPath = EditorUtility.OpenFilePanel("Load Graph", dataPathWithoutAssets + ConstantKeys.DataPath_GameFlow,
				"asset");

			string finalPath = graphPath.Substring(appPathLength - 6);

			nodeGraph = (AG_Graph)AssetDatabase.LoadAssetAtPath(finalPath, typeof(AG_Graph));

			if (nodeGraph == null)
			{
				EditorUtility.DisplayDialog("Node Message:", "Unable to load the graph", "OK");
			}

			var currentNodeEditorWindow = EditorWindow.GetWindow<AG_GameFlowMainWindow>();
			currentNodeEditorWindow.currentGraph = nodeGraph;
		}

		public static void DeleteNode(AG_Node currentNode, AG_Graph currentGraph)
		{
			if (currentGraph != null)
			{
				currentGraph.nodes.Remove(currentNode);
				GameObject.DestroyImmediate(currentNode, true);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

        #endregion

    }
}
        