using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NodeBasedEditor
{
    [Serializable]
    public class NodePropertyView : ViewBase
    {
        #region Public Variables

        #endregion

        #region Protected Variables

        #endregion

        #region Main Methods

        public override void UpdateView(Rect editorRect, Rect percentageRect, Event e, NodeGraph currentGraph)
        {
            base.UpdateView(editorRect, percentageRect, e, currentGraph);

            GUI.Box(ViewRect, ViewTitle, ViewSkin.GetStyle("ViewBG"));

            GUILayout.BeginArea(ViewRect);

            GUILayout.Space(30f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(30f);

            if (CurrentGraph == null || !CurrentGraph.ShowProperties)
            {
                EditorGUILayout.LabelField("NONE");
            }
            else
            {
                CurrentGraph.SelectedNode.DrawNodeProperties(ViewRect);
            }

            GUILayout.Space(30f);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            ProcessEvents(e);
        }

        public override void ProcessEvents(Event e)
        {
            base.ProcessEvents(e);
        }

        #endregion

        #region Utility Methods

        #endregion

        #region Constructors

        public NodePropertyView() : base("Property View") { }

        #endregion
    }
}
