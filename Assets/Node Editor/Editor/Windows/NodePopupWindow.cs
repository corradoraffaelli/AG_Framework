using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace NodeBasedEditor
{
    public class NodePopupWindow : EditorWindow
    {
        private static NodePopupWindow currentPopupWindow;
        private string wantedName = "Valid";

        public static void InitNodePopup()
        {
            currentPopupWindow = EditorWindow.GetWindow<NodePopupWindow>();
            currentPopupWindow.title = "Node popup";
        }

        private void OnGUI()
        {
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginVertical();

            EditorGUILayout.LabelField("Create New Graph", EditorStyles.boldLabel);

            wantedName = EditorGUILayout.TextField("Enter Name: ", wantedName);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Create Graph", GUILayout.Height(40f)))
            {
                if (!String.IsNullOrEmpty(wantedName) && wantedName != "Enter a name...")
                {
                    NodeUtils.CreateNodeGraph(wantedName);
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
