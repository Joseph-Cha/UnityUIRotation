using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ComponentsTreeNode
{
    public ComponentsTreeNode(string _name)
    {
        Name = _name;
    }
    public string Name;
    [SerializeField]
    public List<ComponentInfo> ComponentInfos = new List<ComponentInfo>();
    [SerializeField]
    public List<ComponentsTreeNode> Children = new List<ComponentsTreeNode>();  
}
