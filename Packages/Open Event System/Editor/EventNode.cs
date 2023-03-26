using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace OpenEvents
{ 
    // Node is a GraphElement
    public class EventNode : Node
    {
        public string GUID { get; protected set; }

        public Vector2 Position;

        private List<EventNode> _outputConnections; 
        private List<EventNode> _inputConnections;
        public List<EventNode> OutputConnections
        {
            get
            {
                return _outputConnections;
            }
            protected set { _outputConnections = value; }
        }
        public List<EventNode> InputConnections
        {
            get
            {
                return _inputConnections;
            }
            protected set { _inputConnections = value; }
        }


        public EventNode(string NodeID)
        {
            GUID = NodeID;
            viewDataKey = GUID;
            Position = new Vector2();

            style.left = Position.x;
            style.top = Position.y;

            OutputConnections = new List<EventNode>();
            InputConnections = new List<EventNode>();
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Position.x = newPos.xMin;
            Position.y = newPos.yMin;
        }
    }

}