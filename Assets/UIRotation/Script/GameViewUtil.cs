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
    private static int screenIndex = 16; // Because have 16 indexes in my list.
    private static int gameViewProfilesCount;

    static GameViewUtil()
    {
        // gameViewSizesInstance  = ScriptableSingleton<GameViewSizes>.instance;
        var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
        var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var instanceProp = singleType.GetProperty("instance");
        getGroup = sizesType.GetMethod("GetGroup");
        gameViewSizesInstance = instanceProp.GetValue(null, null);
    }

    private enum GameViewSizeType
    {
        AspectRatio, FixedResolution
    }

    [MenuItem("RectTransformProperty/Screen : Landscape screen #h")]
    public static void SetLandscapeScreen() => SetScreen(2240, 1260);
    
    [MenuItem("RectTransformProperty/Screen : Portrait screen #v")]
    public static void SetPortraitScreen() => SetScreen(1260, 2240);
    
    private static void SetScreen(int width, int height)
    {
        if(!SizeExists(GameViewSizeGroupType.Android, width, height))
        {
            AddCustomSize(GameViewSizeType.FixedResolution, GameViewSizeGroupType.Android, width, height, width > height ? "ViewLandScape" : "ViewPortrait");
        }
        int num = FindSize(GameViewSizeGroupType.Android, width, height);
        SetSize(num);
    }
    
    private static void SetSize(int index)
    {
        var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        var gvWnd = EditorWindow.GetWindow(gvWndType);
        var SizeSelectionCallback = gvWndType.GetMethod("SizeSelectionCallback",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        SizeSelectionCallback.Invoke(gvWnd, new object[] { index, null });
    }

    static object GetGroup(GameViewSizeGroupType type)
    {
        return getGroup.Invoke(gameViewSizesInstance, new object[] { (int)type });
    }

    private static void AddCustomSize(GameViewSizeType viewSizeType, GameViewSizeGroupType sizeGroupType, int width, int height, string text)
    {
        var group = GetGroup(sizeGroupType);
        var addCustomSize = getGroup.ReturnType.GetMethod("AddCustomSize"); // or group.GetType().
        var gvsType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
        string assemblyName = "UnityEditor.dll";
        Assembly assembly = Assembly.Load(assemblyName);
        Type gameViewSize = assembly.GetType("UnityEditor.GameViewSize");
        Type gameViewSizeType = assembly.GetType("UnityEditor.GameViewSizeType");
        ConstructorInfo ctor = gameViewSize.GetConstructor(new Type[]
        {
            gameViewSizeType,
            typeof(int),
            typeof(int),
            typeof(string)
        });
        var newSize = ctor.Invoke(new object[] { (int)viewSizeType, width, height, text });
        addCustomSize.Invoke(group, new object[] { newSize });
    }



    public static bool SizeExists(GameViewSizeGroupType sizeGroupType, int width, int height)
    {
        return FindSize(sizeGroupType, width, height) != -1;
    }
    public static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height)
    {
        // goal:
        // GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
        // int sizesCount = group.GetBuiltinCount() + group.GetCustomCount();
        // iterate through the sizes via group.GetGameViewSize(int index)

        var group = GetGroup(sizeGroupType);
        var groupType = group.GetType();
        var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
        var getCustomCount = groupType.GetMethod("GetCustomCount");
        int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
        var getGameViewSize = groupType.GetMethod("GetGameViewSize");
        var gvsType = getGameViewSize.ReturnType;
        var widthProp = gvsType.GetProperty("width");
        var heightProp = gvsType.GetProperty("height");
        var indexValue = new object[1];
        for (int i = 0; i < sizesCount; i++)
        {
            indexValue[0] = i;
            var size = getGameViewSize.Invoke(group, indexValue);
            int sizeWidth = (int)widthProp.GetValue(size, null);
            int sizeHeight = (int)heightProp.GetValue(size, null);
            if (sizeWidth == width && sizeHeight == height)
                return i;
        }
        return -1;
    }
}
#endif
