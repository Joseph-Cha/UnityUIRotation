using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
//출처 : https://answers.unity.com/questions/956123/add-and-select-game-view-resolution.html
public static class GameViewUtil
{
    static object gameViewSizesInstance;
    static MethodInfo getGroup;

    static GameViewUtil()
    {
        // gameViewSizesInstance  = ScriptableSingleton<GameViewSizes>.instance;
        var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
        var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var instanceProp = singleType.GetProperty("instance");
        getGroup = sizesType.GetMethod("GetGroup");
        gameViewSizesInstance = instanceProp.GetValue(null, null);
    }

    public enum GameViewSizeType
    {
        AspectRatio, FixedResolution
    }

    
    [MenuItem("RectTransformProperty/Screen : Landscape screen #h")]
    public static void SetLandscapeScreen()
    {
        int num = 26;
        SetSize(num);
    }


    [MenuItem("RectTransformProperty/Screen : Portrait screen #v")]
    public static void SetPortraitScreen()
    {
        int num = 27;
        SetSize(num);
    }

    public static void SetSize(int index)
    {
        var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var gvWnd = EditorWindow.GetWindow(gvWndType);
        selectedSizeIndexProp.SetValue(gvWnd, index, null);
    }
}
#endif