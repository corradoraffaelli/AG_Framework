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
                                Debug.Log("Left click in node");
								ProcessNodeLeftClick (e, node);
                            } else if (e.button == 1)
                            {
                                Debug.Log("Right click in node");
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
                        Debug.Log("Right click inside window");
						ProcessContextMenuWindow (e);
                    }
                }
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
				menu.AddSeparator("");
				menu.AddItem(new GUIContent("Unload graph"), false, ContextCallback, "2");

				menu.AddSeparator("");
				menu.AddItem(new GUIContent("Float Node"), false, ContextCallback, "3");
				menu.AddItem(new GUIContent("Add Node"), false, ContextCallback, "4");

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
//				NodePopupWindow.InitNodePopup();
				break;
//			case "1":
//				NodeUtils.LoadGraph();
//				break;
//			case "2":
//				NodeUtils.UnloadGraph();
//				break;
			case "3":
//				NodeUtils.CreateNode (CurrentGraph, NodeType.Float, MousePosition);
				currentGraph.nodes.Add(AG_Node.CreateNode (mousePosition, currentGraph));
				break;
			case "4":
//				NodeUtils.CreateNode(CurrentGraph, NodeType.Add, MousePosition);
				break;
//			case "5":
//				NodeUtils.DeleteNode(NodeToDelete, CurrentGraph);
//				break;
			}
		}

        #region Utility Methods

		void ProcessNodeLeftClick(Event e, AG_Node node)
		{

		}

		void ProcessNodeRightClick(Event e, AG_Node node)
		{

		}

        #endregion

    }
}
        