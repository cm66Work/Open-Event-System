using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace OpenEvents
{
    public class GameEventSOEditorWindow : EditorWindow
    {

        private EventGraphView _graphView;

        [MenuItem("Window/Open Event System/Event Graph")]
        public static void Open(GameEventSO gameEventSO)
        {
            GameEventSOEditorWindow window = GetWindow<GameEventSOEditorWindow>("Event Graph");
            //window.titleContent = new GUIContent("Event Graph");
        }

        private void OnEnable()
        {
            ConstructGraph();
            GenerateToolBar();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        private void GenerateToolBar()
        {
            Toolbar toolbar = new Toolbar();

            Button nodeCreateButton = new Button(() =>
            {
                _graphView.CreateNode("Event Node");
            });
            nodeCreateButton.text = "Create Node";

            toolbar.Add(nodeCreateButton);
            rootVisualElement.Add(toolbar);
        }

        private void ConstructGraph()
        {
            _graphView = new EventGraphView(name = "Event Graph");

            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }
    }
}