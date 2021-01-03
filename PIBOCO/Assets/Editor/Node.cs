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
        

        //GUI.BeginGroup(new Rect(nodeRect.position.x, nodeRect.position.y + nodeRect.size.y / 2f, nodeRect.size.x - 10f, nodeRect.size.y), style);
        //GUILayout.BeginArea ( collectiveRect );

        //myInfo.Draw(collectiveRect, style);

        //GUI.Box ( new Rect ( 0, 0, collectiveRect.size.x - 10f, myInfo.height ), "I am box!", style );
        //GUI.Button ( new Rect ( 0, myInfo.height, 100f, myInfo.height ), "I am button!", style );

        //GUILayout.Button ( "", style );
        //myInfo.Draw ( nodeRect, style );

        //GUILayout.EndArea ();
        //GUI.EndGroup();

        Rect baseRect = new Rect(10f, 30f, nodeRect.size.x - 30f, 30f);

        var extra = 40f;
        var topOffset = 55f;

        //baseRect = new Rect(baseRect.position.x, baseRect.position.y, baseRect.size.x, baseRect.size.y);

        //EditorGUI.LabelField ( baseRect, "Weapon" );

        float marginLeft = 5.5f;
        float marginRight = marginLeft - 0f;

        GUI.Box(new Rect(nodeRect.x , nodeRect.y, nodeRect.width , nodeRect.height + topOffset * 3), "", style);

        

        EditorGUI.LabelField(nodeRect, title, style);
        title = EditorGUI.TextField(new Rect(nodeRect.position.x + marginRight * 4f, nodeRect.position.y + topOffset, nodeRect.width - (marginRight * 4f * 2f), nodeRect.size.y / 2f), "Scene Name:", title);

        //scene = (Scene)EditorGUI.ObjectField(new Rect(nodeRect.position.x + marginRight * 4f, nodeRect.position.y + topOffset + (extra), nodeRect.width - (marginRight * 4f * 2f), nodeRect.size.y / 2f), "Behaviour", scene, typeof(Scene), true);

        //moveSpeed = DrawStat("Move Speed:", nodeRect, marginLeft, marginRight, topOffset, 2f, 2f, extra, moveSpeed, 0f, 100f);
        //rotationSpeed = DrawStat("Rotation Speed:", nodeRect, marginLeft, marginRight, topOffset, 2f, 3f, extra, rotationSpeed, 0f, 100f);

        //EditorGUI.Slider(new Rect(nodeRect.position.x + marginRight * 4f, nodeRect.position.y + topOffset + (extra*2), nodeRect.width - (marginRight * 4f * 2f), nodeRect.size.y / 2f), "Rotation Speed", 50f, 0f, 100f);
        //EditorGUI.Slider(new Rect(nodeRect.position.x + marginRight * 4f, nodeRect.position.y + topOffset + (extra*2), nodeRect.width - (marginRight * 4f * 2f), nodeRect.size.y / 2f), "Rotation Speed", 50f, 0f, 100f);
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
