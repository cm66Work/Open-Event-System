using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace OpenEvents
{ 
    public class EventNode : Node
    {
        public string GUID { get; protected set; }

        public Vector2 Position;

        public EventNode(string NodeID)
        {
            GUID = NodeID;
            viewDataKey = GUID;
            Position = new Vector2();

            style.left = Position.x;
            style.top = Position.y;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Position.x = newPos.xMin;
            Position.y = newPos.yMin;
        }
    }

}