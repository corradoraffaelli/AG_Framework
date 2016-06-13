using UnityEngine;
using System.Collections;
using System.ComponentModel;
using UnityEditor;

namespace NodeBasedEditor
{
    public class NodeEditorWindow : EditorWindow
    {
        #region Variables

        public static NodeEditorWindow CurrentWindow { get; set; }
        public NodePropertyView PropertyView { get; set; }
        public NodeWorkView WorkView { get; set; }
        public NodeGraph CurrentGraph { get; set; }

        public float ViewPercentage = 0.75f;
        #endregion

        #region Main Methods

        public static void InitEditorWindow()
        {
            CurrentWindow = EditorWindow.GetWindow<NodeEditorWindow>();
            CurrentWindow.title = "Node editor";

            CreateViews();
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
            // Check for null views
            if (PropertyView == null || WorkView == null)
            {
                CreateViews();
            }

            // Get and process the current event
            Event e = Event.current;
            ProcessEvents(e);

            // Update
            WorkView.UpdateView(position, new Rect(0f, 0f, ViewPercentage, 1f), e, CurrentGraph);
            PropertyView.UpdateView(new Rect(position.width, position.y, position.width, position.height),
                                    new Rect(ViewPercentage, 0f, 1f - ViewPercentage, 1f), e, CurrentGraph);

            Repaint();
        }

        private void ProcessEvents(Event e)
        {
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftArrow)
            {
                ViewPercentage -= 0.01f;
            }

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.RightArrow)
            {
                ViewPercentage += 0.01f;
            }
        }

        #endregion

        #region Utility Methods

        private static void CreateViews()
        {
            if (CurrentWindow == null)
            {
                CurrentWindow = EditorWindow.GetWindow<NodeEditorWindow>();
            }

            CurrentWindow.PropertyView = new NodePropertyView();
            CurrentWindow.WorkView = new NodeWorkView();
        }

        #endregion
    }
}
