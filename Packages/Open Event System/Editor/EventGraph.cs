using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

namespace OpenEvents
{

    public class EventGraph : GraphView
    {
        
        private readonly Vector2 _defaultNodeSize = new Vector2(x:150, y:200);

        private EventNode _eventNode;

        /// <summary>
        /// Used to keep track of Event Listeners that we have already created Nodes and connections for,
        /// so we can prevent duplicates.
        /// </summary>
        private List<GameEventListener> _eventListnersList;

        #region Graph Creation

        public EventGraph(SerializedObject obj, SerializedProperty prop)
        {
            _eventListnersList = new List<GameEventListener>();
            styleSheets.Add(Resources.Load<StyleSheet>(path: "EventGraphStyleSheet"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            // add the ability to drag/ select drag.
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            GridBackground grid = new GridBackground();
            Insert(index: 0, grid);
            grid.StretchToParentSize();

            BuildEventNodes(obj);



            //CreateNodesForEventListeners(prop);

            
        }

        #endregion


        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge => {
                    EventNode parent = edge.output.node as EventNode;
                    EventNode child = edge.input.node as EventNode;
                    
                });
            }

            return graphViewChange;
        }



        #region Node Creation

        private void BuildEventNodes(SerializedObject obj)
        {
            // Create the main node for the GameEventSO
            EventNode eventSONode = CreateNode(obj.targetObject.name);

            // create Port for broadcaster.
            Port broadcasterPort = CreatePortOnNode(eventSONode, "Broadcaster", Direction.Input);

            eventSONode.SetPosition(new Rect(position: Vector2.zero, size: _defaultNodeSize));


            // Add a button to load connections in the editor.
            // this is done because the list of listeners are created during runtime.
            // this button is not 100% needed is the graph window is opened during runtime as the list is already built.
            Button button = new Button(() => {
                ProcessAllEventListenersComponentsInScene(eventSONode);
                RefreshNodeVisuals(eventSONode);
            });

            button.text = "Load Listeners (Edit mode)";
            eventSONode.titleButtonContainer.Add(button);

            // in-case we have opened the graph view during runtime, we can try to create the listener and broadcaster nodes.
            ProcessAllEventListenersComponentsInScene(eventSONode);


            // Add the eventSONode node to the graph.
            AddElement(eventSONode);

            // lastly refresh the node visuals
            RefreshNodeVisuals(eventSONode);
        }

        private void ProcessAllEventListenersComponentsInScene(EventNode gameEventSONode)
        {
            //TODO :: only process listeners that are using the same GameEventSO as we are.
            foreach(GameEventListener listener in GameObject.FindObjectsOfType<GameEventListener>())
            {
                // if we have already processed this listener then skit it.
                if(_eventListnersList.Contains(listener)) continue;
                _eventListnersList.Add(listener);

                // Create a port on the Output side of the GameEventSO Node.
                Port gameEventSOListenerPort = CreatePortOnNode(gameEventSONode, "Listener", Direction.Output, Port.Capacity.Multi);

                // Create a new Node for the Listener.
                EventNode listenerNode = CreateNode("GameEventListener Component");
                Port listenerInputPort = CreatePortOnNode(listenerNode, "Event Raised", Direction.Input);

                Edge newEdge = gameEventSOListenerPort.ConnectTo(listenerInputPort);


                // --------------------------

                // Add the listenerNode to the graph.
                AddElement(listenerNode);

                // lastly refresh the node visuals
                RefreshNodeVisuals(listenerNode);
            }
        }

        private EventNode CreateNode(string nodeName)
        {
            EventNode node = new EventNode(Guid.NewGuid().ToString());
            node.title = nodeName;
            return node;
        }

        private Port CreatePortOnNode(EventNode node, string portName, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));  // <-- Arbitrary type is used to send data along the connection.
            port.portName = portName;

            if(portDirection == Direction.Input)
                node.inputContainer.Add(port);
            else
                node.outputContainer.Add(port);

            return port;
        }
        #endregion

        #region Node Management

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatablePorts = new List<Port>();

            foreach(Port port in ports)
            {
                if(startPort != port && startPort.node != port.node && startPort.direction != port.direction)
                    compatablePorts.Add(port);
            }
            return compatablePorts;
        }

        private void RefreshNodeVisuals(Node node)
        {
            #region Call after we made changes to a port.
            node.RefreshExpandedState();
            node.RefreshPorts();
            #endregion
        }
        #endregion
    }
}