using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AG_Framework
{
	public class AG_BeginNode : AG_Node {

		public override void InitNode ()
		{
			base.InitNode ();

			nodeSize = new Vector2(150.0f, 80.0f);

			//hasInputEvent = false;
			//hasOutputEvent = true;
		}

	}
}

