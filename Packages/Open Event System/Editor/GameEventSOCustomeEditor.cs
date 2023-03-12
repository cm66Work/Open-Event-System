using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace OpenEvents
{
    public class AssetHandler
    {
        [OnOpenAsset()]
        public static bool OpenEditor(int instanceID, int line)
        {
            GameEventSO gameEventSO = EditorUtility.InstanceIDToObject(instanceID) as GameEventSO;
            if(gameEventSO != null)
            {
                GameEventSOCustomeEditorWindow.Open(gameEventSO);
                return true;
            }    

            return false;
        }
    }
    


    [CustomEditor(typeof(GameEventSO))]
    public class GameEventSOCustomeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("Open Graph View"))
            {
                GameEventSOCustomeEditorWindow.Open((GameEventSO)target);
            }    

            base.OnInspectorGUI();
        }
    }
}