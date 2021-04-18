using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GridLayoutGroup grid;

    [ContextMenu("test")]
    public void GridLayoutGroup()
    {
        object obj = grid;
        Type type = obj.GetType();
        PropertyInfo info = type.GetProperty("padding", BindingFlags.Public | BindingFlags.Instance);
        object obj2 = info.PropertyType;
        RectOffsetStore store = null;

        switch(obj2)
        {
            case RectOffset rect:
                store = new RectOffsetStore(rect.left, rect.right, rect.top, rect.bottom);
                break;
        }
        string json = JsonUtility.ToJson(store);
        Debug.Log(json);
    }

}
[Serializable]
public class RectOffsetStore
{
    public RectOffsetStore(int left, int right, int top, int bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }
    public int Left;
    public int Right;
    public int Top;
    public int Bottom;
}