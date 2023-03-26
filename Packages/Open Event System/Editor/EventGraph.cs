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

        private EventNode _gameEventSONode;
        private List<EventNode> _listenerEventNodes;

        /// <summary>
        /// Used to keep track of Event Listeners that we have already created Nodes and connections for,
        /// so we can prevent duplicates.
        /// </summary>
        private List<GameEventListener> _eventListnersList;

        private GameEventSO _gameEventSO;

        #region Graph Creation

        public EventGraph(SerializedObject obj, SerializedProperty prop)
        {
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;


            _eventListnersList = new List<GameEventListener>();
            _listenerEventNodes = new List<EventNode>();
            _gameEventSO = obj.targetObject as GameEventSO;
            styleSheets.Add(Resources.Load<StyleSheet>(path: "EventGraphStyleSheet"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            // add the ability to drag/ select drag.
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            GridBackground grid = new GridBackground();
            Insert(index: 0, grid);
            grid.StretchToParentSize();


            // Build Nodes for the GameEventSO and its EventListeners
            BuildGameEventSONode();
            BuildGameEventListenersNode();
            // create links
            BuildNodeConnections();


        }

        #endregion


        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.edgesToCreate != null)
            {
                foreach(Edge edge in graphViewChange.edgesToCreate)
                {
                    



                    //AddElement(edge);
                }

                //graphViewChange.edgesToCreate.ForEach(edge => {
                //    EventNode parent = edge.output.node as EventNode;
                //    EventNode child = edge.input.node as EventNode;

                //});
            }

            return graphViewChange;
        }



        #region Node Creation

        // Create Node that will represent the GameEventSO
        private void BuildGameEventSONode()
        {
            _gameEventSONode = CreateNode(_gameEventSO.name);

            _gameEventSONode.SetPosition(new Rect(position: new Vector2(_defaultNodeSize.x, _defaultNodeSize.y), size: _defaultNodeSize));

            // GameEventSONode will have an Input(broadcaster) and an Output(listeners)
            // add Input Port
            CreatePortOnNode(_gameEventSONode, "Broadcaster", Direction.Input);


            // Add the eventSONode node to the graph.
            AddElement(_gameEventSONode);
        }

        private void BuildGameEventListenersNode()
        {
            EventNode lastNode = null;
            foreach(GameEventListener listener in GameObject.FindObjectsOfType<GameEventListener>())
            {
                if(listener.GameEventSO != _gameEventSO) continue;

                EventNode node = CreateNode(listener.gameObject.name);

                // stack nodes
                Vector2 position = (lastNode == null) ? _gameEventSONode.Position : lastNode.Position;

                position.x += (lastNode == null) ? (_defaultNodeSize.x * 2) : 0;
                position.y += (lastNode == null) ? 0 : _defaultNodeSize.y / 2.5f;

                node.SetPosition(new Rect(position, size: _defaultNodeSize));


                // Add the eventSONode node to the graph.
                AddElement(node);
                _listenerEventNodes.Add(node);

                lastNode = node;
            }
        }

        private void BuildNodeConnections()
        {
            Port outputPort = CreatePortOnNode(_gameEventSONode, "Listener", Direction.Output, capacity: Port.Capacity.Multi);

            // Add Output Ports to The GameEventSONode
            for(int i = 0; i < _listenerEventNodes.Count; i++)
            {
                
                Port inputport = CreatePortOnNode(_listenerEventNodes[i], "Trigger", Direction.Input);


                Edge edge = outputPort.ConnectTo(inputport);

                AddElement(edge);

                RefreshNodeVisuals(_listenerEventNodes[i]);
            }

            RefreshNodeVisuals(_gameEventSONode);
        }


        private void BuildEventNodes(SerializedObject obj)
        {
            // Create the main node for the GameEventSO
            EventNode eventSONode = CreateNode(obj.targetObject.name);

            // create Port for broadcaster (Input).
            CreatePortOnNode(eventSONode, "Broadcaster", Direction.Input);

            eventSONode.SetPosition(new Rect(position: Vector2.zero, size: _defaultNodeSize));


            // Add a button to load connections in the editor.
            // this is done because the list of listeners are created during runtime.
            // this button is not 100% needed is the graph window is opened during runtime as the list is already built.
            Button button = new Button(() => {
                //ProcessAllEventListenersComponentsInScene(eventSONode);
                RefreshNodeVisuals(eventSONode);
            });

            button.text = "Load Listeners (Edit mode)";
            eventSONode.titleButtonContainer.Add(button);

            // in-case we have opened the graph view during runtime, we can try to create the listener and broadcaster nodes.
            //ProcessAllEventListenersComponentsInScene(eventSONode);


            // Add the eventSONode node to the graph.
            AddElement(eventSONode);

            // lastly refresh the node visuals
            RefreshNodeVisuals(eventSONode);
        }

        private EventNode CreateNode(string nodeName)
        {
            EventNode node = new EventNode( Guid.NewGuid().ToString() );
            node.title = nodeName;
            return node;
        }

        private Port CreatePortOnNode(EventNode node, string portName, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(EventNode));  // <-- Arbitrary type is used to send data along the connection.
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