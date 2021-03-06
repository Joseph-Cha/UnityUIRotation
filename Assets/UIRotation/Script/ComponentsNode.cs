using System;
using System.Collections.Generic;

[Serializable]
public class ComponentsNode
{
    public ComponentsNode(string name)
    {
        this.Name = name;
    }
    public string Name;
    public List<ComponentInfo> ComponentInfos = new List<ComponentInfo>();
    public List<ComponentsNode> Children = new List<ComponentsNode>();
}
