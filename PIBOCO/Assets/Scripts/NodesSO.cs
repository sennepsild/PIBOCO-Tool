using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Node List", menuName = "ScriptableObjects/NodeData", order = 3)]
public class NodesSO : ScriptableObject
{
    
    public List<Vector2> NodePositions;
    public List<string> Nodetitles;


    public List<int> NodeNumberin;
    public List<int> NodeNumberOut;
    public List<int> NumberOut;
}
