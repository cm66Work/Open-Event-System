using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

namespace OpenEvents
{

    public class EventGraphView : GraphView
    {
        private readonly Vector2 defaultNodeSize = new Vector2(x: 150, y: 200);

        public EventGraphView(string name)
        {
            styleSheets.Add(Resources.Load<StyleSheet>(path: "EventGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            // add the ability to drag/ select drag.
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            GridBackground grid = new GridBackground();
            Insert(index: 0, grid);
            grid.StretchToParentSize();

            AddElement(GenerateEntryPoint());
        }

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

        private Port GeneratePort(EventNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float)); // Arbitrary type is used to send data along the connection.
        }

        private EventNode GenerateEntryPoint()
        {
            EventNode node = new EventNode
            {
                title = "start",
                GUID = Guid.NewGuid().ToString(),
                DialogText = "ENTRYPOINT",
                EntryPoint = true
            };

            Port generatedPort = GeneratePort(node, Direction.Output);
            generatedPort.portName = "Next";
            node.outputContainer.Add(generatedPort);


            RefreshNodeVisuals(node);

            node.SetPosition(new Rect(100, 200, 100, 150));

            return node;
        }

        public void CreateNode(string nodeName)
        {
            AddElement(CreateEventNode(nodeName));
        }

        public EventNode CreateEventNode(string nodeName)
        {
            EventNode newNode = new EventNode
            {
                title = nodeName,
                DialogText = nodeName,
                GUID = Guid.NewGuid().ToString()
            };

            Port inputPort = GeneratePort(newNode, Direction.Input, Port.Capacity.Multi);

            inputPort.portName = "Input";
            newNode.inputContainer.Add(inputPort);

            Button button = new Button(() => { AddChoicePort(newNode); });
            button.text = "New Choice";
            newNode.titleButtonContainer.Add(button);

            RefreshNodeVisuals(newNode);

            newNode.SetPosition(new Rect(position: Vector2.zero, size: defaultNodeSize));

            return newNode;
        }

        private void AddChoicePort(EventNode eventNode)
        {
            Port generatedPort = GeneratePort(eventNode, Direction.Output);

            int outPutPortCount = eventNode.outputContainer.Query(name: "connector").ToList().Count;
            string outputPortName = $"Choice {outPutPortCount}";

            eventNode.outputContainer.Add(generatedPort);

            RefreshNodeVisuals(eventNode);
        }

        private void RefreshNodeVisuals(Node node)
        {
            #region Call after we made changes to a port.
            node.RefreshExpandedState();
            node.RefreshPorts();
            #endregion
        }

    }
}