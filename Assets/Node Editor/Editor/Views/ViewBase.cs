
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace NodeBasedEditor
{
    [Serializable]
    public class ViewBase
    {
        #region Public Variables

        public string ViewTitle { get; set; }
        public Rect ViewRect { get; set; }

        #endregion

        #region Protected Variables

        protected GUISkin ViewSkin { get; set; }
        protected NodeGraph CurrentGraph { get; set; }

        #endregion

        #region Constructors

        public ViewBase(string title)
        {
            ViewTitle = title;
            SetupEditorSkin();
        }

        #endregion

        #region Main Methods

        public virtual void UpdateView(Rect editorRect, Rect percentageRect, Event e, NodeGraph currentGraph)
        {
            if (ViewSkin == null)
            {
                SetupEditorSkin();
            }

            CurrentGraph = currentGraph;

            if (currentGraph != null)
            {
                ViewTitle = currentGraph.GraphName;
            }
            else
            {
                ViewTitle = "No Graph";
            }

            ViewRect = new Rect(editorRect.x * percentageRect.x,
                                editorRect.y * percentageRect.y,
                                editorRect.width * percentageRect.width,
                                editorRect.height * percentageRect.height);
        }

        public virtual void ProcessEvents(Event e)
        {
            
        }

        #endregion

        #region Utility Methods

        protected void SetupEditorSkin()
        {
            ViewSkin = Resources.Load<GUISkin>("GUI Skins/Editor Skins/Node Editor Skin");
        }
        #endregion

    }
}
