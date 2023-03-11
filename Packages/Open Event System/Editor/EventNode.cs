using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace OpenEvents
{

    public class EventNode : Node
    {
        public string GUID;
        public string DialogText;
        public bool EntryPoint = false;
    }
}