using UnityEngine;
using System.Collections;
using UnityEditor;

namespace AG_Framework
{
	public class AG_CreateGraphWindow : EditorWindow
	{
		private static AG_CreateGraphWindow currentPopupWindow;
		private string wantedName = "Chapter name";

		public static void InitNodePopup()
		{
			currentPopupWindow = EditorWindow.GetWindow<AG_CreateGraphWindow>();
			currentPopupWindow.titleContent = new GUIContent ("New chapter");
		}

		private void OnGUI()
		{
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Space(10);

			GUILayout.BeginVertical();

			EditorGUILayout.LabelField("Create New Graph", EditorStyles.boldLabel);

			wantedName = EditorGUILayout.TextField("Enter Name: ", wantedName);

			GUILayout.Space(10);

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Create Graph", GUILayout.Height(40f)))
			{
				
				if (!string.IsNullOrEmpty(wantedName) && wantedName != "Enter a name...")
				{
//					NodeUtils.CreateNodeGraph(wantedName);
					var currentWorkView = AG_GameFlowMainWindow.currentWindow = EditorWindow.GetWindow<AG_GameFlowMainWindow>();
					if (currentWorkView != null)
					{
						currentWorkView.currentGraph = AG_Graph.CreateNodeGraph(wantedName);

						if (currentWorkView.currentGraph != null) {
							AssetDatabase.CreateAsset(currentWorkView.currentGraph, ConstantKeys.DataPath_GameFlow + currentWorkView.currentGraph.graphName + ".asset");
							AssetDatabase.SaveAssets();
							AssetDatabase.Refresh();
						} else {
							EditorUtility.DisplayDialog("Node Message:", "Unable to create graph", "OK");
						}

					}
					currentPopupWindow.Close();
				}
				else
				{
					EditorUtility.DisplayDialog("Node Message: ", "Please enter a valid graph name!", "OK");
				}
			}

			if (GUILayout.Button("Cancel", GUILayout.Height(40f)))
			{
				currentPopupWindow.Close();
			}

			GUILayout.EndHorizontal();

			GUILayout.EndVertical();

			GUILayout.Space(20);
			GUILayout.EndHorizontal();
			GUILayout.Space(20);
		}
	}
}
