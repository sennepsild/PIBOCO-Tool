using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Node
{

    //public Scene scene;

    public Rect nodeRect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    public ConnectionPoint inPoint;
    public ConnectionPoint[] outPoints;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;


    public int Connections =2;

    public Action<Node> OnRemoveNode;
    
    public void Init(Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        inPoint.OnClickConnectionPoint = OnClickInPoint;
        for (int i = 0; i < outPoints.Length; i++)
        {
            outPoints[i].OnClickConnectionPoint = OnClickOutPoint;
        }
        OnRemoveNode = OnClickRemoveNode;
    }

    public Node( Vector2 position, float width, float height,string title, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        this.title = title;
        nodeRect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint,0);


        outPoints = new ConnectionPoint[Connections];
        for (int i = 0; i < Connections; i++)
        {
            outPoints[i] = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint,i);

        }

        
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
    }

    public void Drag(Vector2 delta)
    {
        nodeRect.position += delta;
        
    }

    public void Draw()
    {
        inPoint.Draw();
        for (int i = 0; i < outPoints.Length; i++)
        {
            outPoints[i].Draw();
        }
        
   
      

        Rect baseRect = new Rect(10f, 30f, nodeRect.size.x - 30f, 30f);

        
        var topOffset = 55f;
        

        float marginLeft = 5.5f;
        float marginRight = marginLeft - 0f;

        GUI.Box(new Rect(nodeRect.x , nodeRect.y, nodeRect.width , nodeRect.height + topOffset ), "", style);

        

        EditorGUI.LabelField(nodeRect, title, style);
        title = EditorGUI.TextField(new Rect(nodeRect.position.x + marginRight * 4f, nodeRect.position.y + topOffset, nodeRect.width - (marginRight * 4f * 2f), nodeRect.size.y / 2f), "Scene Name:", title);
        
    }

    public bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (nodeRect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }
                if (e.button == 1 && isSelected && nodeRect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;

            case EventType.KeyDown:
                if(e.keyCode == KeyCode.Delete)
                {
                    if (isSelected)
                    {
                        OnClickRemoveNode();
                        GUI.changed = true;
                    }
                }
                break;
        }
        return false;
    }

    
    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }
    

    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }
    
}
