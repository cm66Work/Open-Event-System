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
        public static bool OpenEditor(int instanceId, int line)
        {
            GameEventSO gameEventSO = EditorUtility.InstanceIDToObject(instanceId) as GameEventSO;
            if(gameEventSO != null)
            {
                GameEventSOEditorWindow.Open(gameEventSO);
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
            if(GUILayout.Button("Open Graph"))
            {
                GameEventSOEditorWindow.Open((GameEventSO)target);
            }
        }
    }
}