using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace OpenEvents
{

    public class GameEventSOCustomeEditorWindow : EditorWindow
    {
        private EventGraph _eventGraph;

        private SerializedObject _serializedObject;
        private SerializedProperty _currentProperty;

        public static void Open(GameEventSO gameEventSO)
        {
            GameEventSOCustomeEditorWindow window = GetWindow<GameEventSOCustomeEditorWindow>($"Event Graph");
            window._serializedObject = new SerializedObject(gameEventSO);

            window.ConstructGraph();
            //window.GenerateToolBar();
        }


        ///// <summary>
        ///// Gets triggered when the editor window opens.
        ///// </summary>
        private void ConstructGraph()
        {

            _currentProperty = _serializedObject.FindProperty("Listeners");
            _eventGraph = new EventGraph(_serializedObject, _currentProperty);



            _eventGraph.StretchToParentSize();
            rootVisualElement.Add(_eventGraph);
        }

        /// <summary>
        /// Gets triggered when the editor window closes.
        /// </summary>
        private void OnDisable()
        {
            // remove reference to the GameEventSO to protect it from accidental modification whilst the window is closed
            rootVisualElement.Remove(_eventGraph);
        }


        // TODO:: This is currently redundant as we wont allow the users to create new blank nodes... maybe.
        private void GenerateToolBar()
        {
            Toolbar toolbar = new Toolbar();

            Button nodeCreateButton = new Button(() =>
            {
                //_eventGraph.CreateNode("Event Node");
            });
            nodeCreateButton.text = "Create Node";

            toolbar.Add(nodeCreateButton);
            rootVisualElement.Add(toolbar);
        }
    }
}