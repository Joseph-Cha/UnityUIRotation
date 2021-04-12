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
    public string Name;
    public List<ComponentInfo> ComponentInfos = new List<ComponentInfo>();
    public List<ComponentsNode> Children = new List<ComponentsNode>();  
}
