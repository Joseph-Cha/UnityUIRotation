using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using UnityEngine.UI;

[Serializable]
public class ComponentInfo
{
    public ComponentInfo(Component[] components)
    {
        components.ToList().ForEach(component =>
        {
            switch(component)
            {
                case RectTransform rect:
                {
                    RectInfos = rect.GetType().GetProperties();
                    RectInfos[0].GetType();
                }
                break;
                case LayoutElement le:
                {
                    LayoutElementInfos = le.GetType().GetProperties();
                }
                break;
            }
        });
    }
    public PropertyInfo[] RectInfos;
    public PropertyInfo[] LayoutElementInfos;
}
