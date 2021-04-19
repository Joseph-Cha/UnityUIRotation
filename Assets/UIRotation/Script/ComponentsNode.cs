using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ComponentsNode
{
    public ComponentsNode(string _name)
    {
        Name = _name;
    }
    [SerializeField]   
    public string Name;
    [SerializeField]   
    public List<ComponentInfo> ComponentInfos = new List<ComponentInfo>();
    [SerializeField]   
    public List<ComponentsNode> Children = new List<ComponentsNode>();  
}
