using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NodeBasedEditor
{
    public class AddNode : NodeBase
    {
        public float NodeSum;
        public NodeOutput Output;
        public NodeInput InputOne;
        public NodeInput InputTwo;

        public AddNode()
        {
            Output = new NodeOutput();
            InputOne = new NodeInput();
            InputTwo = new NodeInput();
        }

        public override void InitNode()
        {
            base.InitNode();
            NodeType = NodeType.Add;
            NodeRect = new Rect(10f, 10f, 200f, 65f);
        }

        public override void UpdateNode(Event e, Rect viewRect)
        {
            base.UpdateNode(e, viewRect);
        }

#if UNITY_EDITOR

        public override void UpdateNodeGUI(Event e, Rect viewRect, GUISkin guiSkin)
        {
            base.UpdateNodeGUI(e, viewRect, guiSkin);

            // Output
            if (GUI.Button(new Rect(NodeRect.x + NodeRect.width, NodeRect.y + NodeRect.height * 0.5f - 12f, 24f, 24f), "", guiSkin.GetStyle("NodeOutput")))
            {
                if (ParentGraph != null)
                {
                    ParentGraph.WantsConnection = true;
                    ParentGraph.ConnectionNode = this;
                }
            }

            // Inp A
            if (GUI.Button(new Rect(NodeRect.x - 24f, NodeRect.y + (NodeRect.height * 0.33f) * 2f - 8f, 24f, 24f), "", guiSkin.GetStyle("NodeInput")))
            {
                if (ParentGraph != null)
                {
                    InputOne.inputNode = ParentGraph.ConnectionNode;
                    InputOne.isOccupied = InputOne.inputNode != null;

                    ParentGraph.WantsConnection = false;
                    ParentGraph.ConnectionNode = null;

                    EditorUtility.SetDirty(this);
                }
            }

            // Inp B
            if (GUI.Button(new Rect(NodeRect.x - 24f, NodeRect.y + (NodeRect.height * 0.33f) - 14f, 24f, 24f), "", guiSkin.GetStyle("NodeInput")))
            {
                if (ParentGraph != null)
                {
                    InputTwo.inputNode = ParentGraph.ConnectionNode;
                    InputTwo.isOccupied = InputTwo.inputNode != null;

                    ParentGraph.WantsConnection = false;
                    ParentGraph.ConnectionNode = null;
                }
            }

            DrawInputLines();
        }

        private void DrawInputLines()
        {
            Handles.BeginGUI();

            Handles.color = Color.white;

            if (InputOne.inputNode != null && InputOne.isOccupied)
            {
                DrawLine(InputOne, 2f);
            }
            else
            {
                InputOne = new NodeInput();
            }

            if (InputTwo.inputNode != null && InputTwo.isOccupied)
            {
                DrawLine(InputTwo, 1f);
            }
            else
            {
                InputTwo = new NodeInput();
            }

            Handles.EndGUI();
        }

        private void DrawLine(NodeInput currentInput, float inputId)
        {
            Handles.DrawLine(new Vector3(
                currentInput.inputNode.NodeRect.x + currentInput.inputNode.NodeRect.width + 24f,
                currentInput.inputNode.NodeRect.y + currentInput.inputNode.NodeRect.height * 0.5f, 0f),
                new Vector3(NodeRect.x - 24f, NodeRect.y + (NodeRect.height * 0.33f) * inputId, 0f));
        }

        public override void DrawNodeProperties(Rect viewRect)
        {
            base.DrawNodeProperties(viewRect);

            if (InputOne.isOccupied && InputTwo.isOccupied)
            {
                //EditorGUILayout.LabelField("Value :", (((FloatNode)InputOne.inputNode).NodeValue + ((FloatNode)InputTwo.inputNode).NodeValue).ToString("F"));
            }
        }

#endif
    }
}
