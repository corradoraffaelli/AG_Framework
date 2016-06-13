using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using UnityEditor;

namespace NodeBasedEditor
{
    [Serializable]
    public class NodeWorkView : ViewBase
    {
        #region Public Variables

        #endregion

        #region Protected Variables

        protected Vector2 MousePosition;
        protected NodeBase NodeToDelete;

        #endregion

        #region Main Methods

        public override void UpdateView(Rect editorRect, Rect percentageRect, Event e, NodeGraph currentGraph)
        {
            base.UpdateView(editorRect, percentageRect, e, currentGraph);

            GUI.Box(ViewRect, ViewTitle, ViewSkin.GetStyle("ViewBG"));

            NodeUtils.DrawGrid(ViewRect, 60f, 0.15f, Color.white);
            NodeUtils.DrawGrid(ViewRect, 20f, 0.05f, Color.white);

            GUILayout.BeginArea(ViewRect);

            if (currentGraph != null)
            {
                currentGraph.UpdateGraphGUI(e, ViewRect, ViewSkin);
            }

            GUILayout.EndArea();

            ProcessEvents(e);
        }

        public override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);

            if (ViewRect.Contains(e.mousePosition))
            {
                if (e.button == 0)
                {
                    if (e.type == EventType.MouseDown)
                    {
                    }

                    if (e.type == EventType.MouseDrag)
                    {
                    }

                    if (e.type == EventType.MouseUp)
                    {
                    }
                }

                if (e.button == 1)
                {
                    if (e.type == EventType.MouseDown)
                    {
                        MousePosition = e.mousePosition;
                        NodeToDelete = null;
                        if (CurrentGraph != null)
                        {
                            NodeToDelete = CurrentGraph.Nodes.FirstOrDefault(x => x.NodeRect.Contains(e.mousePosition));
                        }

                        if (NodeToDelete == null)
                        {
                            ProcessContextMenu(e, 0);
                        }
                        else
                        {
                            ProcessContextMenu(e, 1);
                        }

                    }
                }
            }
        }

        #endregion

        #region Utility Methods

        private void ProcessContextMenu(Event e, int contextMenuID)
        {
            GenericMenu menu = new GenericMenu();

            if (contextMenuID == 0)
            {
                menu.AddItem(new GUIContent("Create graph"), false, ContextCallback, "0");
                menu.AddItem(new GUIContent("Load graph"), false, ContextCallback, "1");

                if (CurrentGraph != null)
                {
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Unload graph"), false, ContextCallback, "2");

                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Float Node"), false, ContextCallback, "3");
                    menu.AddItem(new GUIContent("Add Node"), false, ContextCallback, "4");

                }
            }
            if (contextMenuID == 1)
            {
                if (CurrentGraph != null)
                {
                    menu.AddItem(new GUIContent("Remove Node"), false, ContextCallback, "5");
                }

            }

            menu.ShowAsContext();
            e.Use();
        }

        private void ContextCallback(object obj)
        {
            switch (obj.ToString())
            {
                case "0":
                    NodePopupWindow.InitNodePopup();
                    break;
                case "1":
                    NodeUtils.LoadGraph();
                    break;
                case "2":
                    NodeUtils.UnloadGraph();
                    break;
                case "3":
                    NodeUtils.CreateNode(CurrentGraph, NodeType.Float, MousePosition);
                    break;
                case "4":
                    NodeUtils.CreateNode(CurrentGraph, NodeType.Add, MousePosition);
                    break;
                case "5":
                    NodeUtils.DeleteNode(NodeToDelete, CurrentGraph);
                    break;
            }
        }

        #endregion

        #region Constructors

        public NodeWorkView() : base("Work View") { }

        #endregion
    }
}
