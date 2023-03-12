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

        public EventGraph()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(path: "EventGraphStyleSheet"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            // add the ability to drag/ select drag.
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            GridBackground grid = new GridBackground();
            Insert(index: 0, grid);
            grid.StretchToParentSize();
        }
    }
}