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
        /// <summary>
        /// Holds the GameEventSO for the current editor window.
        /// When a new window is opened this is changed to always be relative to the GameEventSO we are working with.
        /// </summary>
        private static GameEventSO _currentGameEventOS;

        private EventGraphView _graphView;

        [MenuItem("Window/Open Event System/Event Graph")]
        public static void Open(GameEventSO gameEventSO)
        {
            GameEventSOEditorWindow window = GetWindow<GameEventSOEditorWindow>("Event Graph");
            _currentGameEventOS = gameEventSO;
            //window.titleContent = new GUIContent("Event Graph");
        }

        /// <summary>
        /// Gets triggered when the editor window opens.
        /// </summary>
        private void OnEnable()
        {
            ConstructGraph();
            GenerateToolBar();
        }

        /// <summary>
        /// Gets triggered when the editor window closes.
        /// </summary>
        private void OnDisable()
        {                               
            // remove reference to the GameEventSO to protect it from accidental modification whilst the window is closed
            _currentGameEventOS = null; 
            rootVisualElement.Remove(_graphView);
        }
        

        // TODO:: This is currently redundant as we wont allow the users to create new blank nodes... maybe.
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