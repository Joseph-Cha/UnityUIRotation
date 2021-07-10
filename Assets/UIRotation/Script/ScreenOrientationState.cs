using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public class ScreenOrientationState
{
    public bool IsLandscape => CurrentOrientaion() == ScreenOrientation.Portrait ? false : true;
    private ScreenOrientation type;
    public string GetPathByOrientation()
    {
        string path = $"JsonData/Portrait";
        type = CurrentOrientaion();
        
        switch (type)
        {
            case ScreenOrientation.LandscapeLeft:
            case ScreenOrientation.LandscapeRight:
                path = $"JsonData/Landscape";
                break;
        }
        return path;
    }
    public string GetPathByOrientation(ScreenOrientation type)
    {
        string path = $"JsonData/Portrait";
        
        switch (type)
        {
            case ScreenOrientation.LandscapeLeft:
            case ScreenOrientation.LandscapeRight:
                path = $"JsonData/Landscape";
                break;
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
