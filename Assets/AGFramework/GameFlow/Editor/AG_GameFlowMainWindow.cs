using UnityEngine;
using System.Collections;
using System.ComponentModel;
using UnityEditor;

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

			//Bottom Area (Chapter choose)
			Rect bottomRect = new Rect(10, position.height - 30.0f, 200, 30.0f);
			GUILayout.BeginArea(bottomRect);

			if (GUILayout.Button("Press me"))
			    Debug.Log("Button pressed");

			GUILayout.EndArea();

            // Get and process the current event
            Event e = Event.current;
            ProcessEvents(e);

            Repaint();
        }

        private void ProcessEvents(Event e)
        {
            //Debug.Log(e.mousePosition +" " + position);


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
//								node.isSelected = true;
								selectedNode = node;
                            } else if (e.button == 1)
                            {
								ProcessNodeRightClick (e, node);
                            }

                            return;
                        }

//						else if (e.type == EventType.MouseDrag)
//						{
//							if (e.button == 0)
//							{
//								selectedNode.MouseDragged (e);
//							} 
//
//							return;
//						}
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
                        Debug.Log("Right click inside window");
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
			if (e.button == 0 && e.type == EventType.mouseUp)
				DeselectAllNodes ();

        }

        #endregion

		void NodesGUI()
		{
			if (currentGraph != null && currentGraph.nodes != null) 
			{
				foreach (AG_Node node in currentGraph.nodes)
				{
					GUI.Box (node.nodeRect, node.nodeName);
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
				menu.AddItem(new GUIContent("Dialogue Node"), false, ContextCallback, "3");
//				menu.AddItem(new GUIContent("Add Node"), false, ContextCallback, "4");

			}


			menu.ShowAsContext();
			e.Use();
		}

		private void ContextCallback(object obj)
		{
			switch (obj.ToString())
			{
			case "0":
				AG_CreateGraphWindow.InitNodePopup ();
				break;
			case "1":
				LoadGraph();
				break;
			case "3":
				{
					AG_Node dialogueNode = AG_Node.CreateNode (mousePosition, currentGraph, AG_NodeType.Dialogue);

					AssetDatabase.AddObjectToAsset (dialogueNode, currentGraph);
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();

					currentGraph.nodes.Add(dialogueNode);

					break;
				}

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
        