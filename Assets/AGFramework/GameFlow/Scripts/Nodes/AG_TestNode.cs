using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AG_Framework
{
    public class AG_TestNode : AG_Node
    {

        public override void InitNode()
        {
            base.InitNode();

            nodeSize = new Vector2(200.0f, 160.0f);

            nodeName = "Test node";

            AddInputPoint("InputTest", true, 200);
            AddInputPoint("InputTest02", false, 1);

            AddOutputPoint("Output", true, 1);
            AddOutputPoint("Output02", true, 10);
            AddOutputPoint("Output03", false, 10);
        }

    }
}