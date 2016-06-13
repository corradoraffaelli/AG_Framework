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
        AG_Graph currentGraph;
        public string graphTitle;

        Rect windowRect;

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
           
            //Bottom Area (Chapter choose)
            //Rect bottomRect = new Rect(10, position.height - 30.0f, 200, 30.0f);
            //GUILayout.BeginArea(bottomRect);

            //if (GUILayout.Button("Press me"))
            //    Debug.Log("Button pressed");

            //GUILayout.EndArea();

            // Get and process the current event
            Event e = Event.current;
            ProcessEvents(e);

            Repaint();
        }

        private void ProcessEvents(Event e)
        {
            //Debug.Log(e.mousePosition +" " + position);
            
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
                    }
                }
            }

        }

        #endregion

        #region Utility Methods


        #endregion

    }
}
        