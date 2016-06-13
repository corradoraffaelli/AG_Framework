using UnityEngine;
using System.Collections;
using UnityEditor;

namespace AG_Framework
{
    public class AG_Menu
    {
        [MenuItem("AG Framework/Scene Builder")]
        public static void OpenSceneBuilderWindow()
        {
            //AG_GameFlowMainWindow.InitEditorWindow();
        }

        [MenuItem("AG Framework/Game Flow")]
        public static void OpenGameFlowWindow()
        {
            AG_GameFlowMainWindow.InitEditorWindow();
        }
        
        [MenuItem("AG Framework/Characters")]
        public static void OpenCharactersWindow()
        {
            //AG_GameFlowMainWindow.InitEditorWindow();
        }

        [MenuItem("AG Framework/GUI")]
        public static void OpenGUIWindow()
        {
            //AG_GameFlowMainWindow.InitEditorWindow();
        }

        [MenuItem("AG Framework/Inventory")]
        public static void OpenInventoryWindow()
        {
            //AG_GameFlowMainWindow.InitEditorWindow();
        }
    }
}
