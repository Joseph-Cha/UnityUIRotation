using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public class ScreenOrientationState
{
    private ScreenOrientation type;
    public string GetPathByOrientation()
    {
        type = CurrentOrientaion();
        string path;
        
        switch (type)
        {
            case ScreenOrientation.Portrait:
                path = $"JsonData/Portrait";
                break;
            case ScreenOrientation.Landscape:
                path = $"JsonData/Landscape";
                break;
            default:
                return null;
        }

        return path;
    }

    public ScreenOrientation CurrentOrientaion()
    {
        #if UNITY_EDITOR
        Vector2 gameView = GetMainGameViewSize();        
        float screenWidth = gameView.x;
        float screenHeight = gameView.y;

        if(screenHeight > screenWidth)
            type = ScreenOrientation.Portrait;
        else
            type  = ScreenOrientation.Landscape;        

        # else
        type = Screen.orientation;
        #endif

        return type;
    }

    private Vector2 GetMainGameViewSize()
    {
        Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        MethodInfo GetSizeOfMainGameView = 
            T?.GetMethod("GetSizeOfMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var SizeOfMainGameView = GetSizeOfMainGameView?.Invoke(null,null);

        return (Vector2)SizeOfMainGameView;
    }
}
