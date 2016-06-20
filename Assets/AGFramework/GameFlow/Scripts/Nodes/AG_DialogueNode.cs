﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AG_Framework
{
	public class AG_DialogueNode : AG_Node {

		public string dialogueLine = "Default Dialogue";

		public override void InitNode ()
		{
			base.InitNode ();

			nodeSize = new Vector2(150.0f, 120.0f);
		}

	}
}


