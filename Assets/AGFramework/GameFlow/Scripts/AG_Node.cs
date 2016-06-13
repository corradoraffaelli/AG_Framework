using UnityEngine;
using System.Collections;

namespace AG_Framework
{
    public class AG_Node : ScriptableObject
    {

        public string nodeName;
        public Rect nodeRect;
        public AG_Graph parentGraph;
        public AG_NodeType nodeType;
        public Vector2 nodeSize;

        public void InitNode()
        {

        }
    }
}


