using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public enum ConnectionPointType { In, Out }

public class ConnectionPoint 
{
    public Rect rect;

    public ConnectionPointType type;

    public Node node;

    public int number;

    public GUIStyle style;

    public Action<ConnectionPoint> OnClickConnectionPoint;

    public ConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint,int number)
    {
        this.number = number;
        this.node = node;
        this.type = type;
        this.style = style;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        rect = new Rect(0, 0, 10f, 20f);
    }

    public void Draw()
    {
        rect.y = node.nodeRect.y + (node.nodeRect.height * 0.5f) - rect.height * 0.5f;
        rect.y += (float)number*40f;
        switch (type)
        {
            case ConnectionPointType.In:
                rect.x = node.nodeRect.x - rect.width + 8f;
                break;

            case ConnectionPointType.Out:
                rect.x = node.nodeRect.x + node.nodeRect.width - 8f;
                break;
        }

        if (GUI.Button(rect, "", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}
