using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System.Collections;

public class ComponentProperty : MonoBehaviour
{
    [SerializeField]
    private Transform Target;

    private void Awake()
    {
        if (Target == null)
            Target = GetComponent<Transform>();
    }

    [ContextMenu("Save")]
    public void Save()
    {
        CreateJsonDirectory();
        List<Component> components = Target.GetComponentsInChildren<Component>(true).Where(component => component.CompareTag("UIProperty")).ToList();
        if (components != null)
            Save(components);
        else
            Debug.LogError("There are no Components tagged \"UIProperty\" in the target");        
    }

    private void Save(IEnumerable<Component> components)
    {
        // Arrage components data
        string path =  $"{Application.dataPath}/Resources/{GetPathByOrientation()}/{Target.name}.json";
        string jsonData = string.Empty;
        ComponentStore componentStore = new ComponentStore();
        components.ToList().ForEach(component => componentStore.Data.Add(new ComponentInfo(component)));

        // Convert components data to json
        try
        {
            jsonData = JsonUtility.ToJson(componentStore);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            return;
        }
        
        // Save a json file to Resources folder
         if(File.Exists(path))
            File.Delete(path);
        File.WriteAllText(path, jsonData);
        #if UNITY_EDITOR
        var relativePath = $"Assets/Resources/{GetPathByOrientation()}/{Target.name}.json";
        AssetDatabase.ImportAsset(relativePath);
        #endif
        Debug.Log($"Save Complete.\nCurrent Orientaion : {CurrentOrientaion()}, File Name : {Target.name}.json" );
    }


    [ContextMenu("Load")]    
    public void Load()
    {
        List<Component> components = Target.GetComponentsInChildren<Component>(true).Where(component => component.CompareTag("UIProperty")).ToList();

        TextAsset jsonFile;
        string resourcePath = $"{GetPathByOrientation()}/{Target.name}";
        jsonFile = Resources.Load<TextAsset>(resourcePath);
        if(jsonFile == null)
        {
            Debug.LogError("It couldn't find saved file. You must save first and try again");
            return;
        }

        try
        {
            var store = JsonUtility.FromJson<ComponentStore>(jsonFile.text);
            for (int i = 0; i < components.Count(); i++)
            {
                store.Data[i].SetPropertyValueByComponent(components[i]);
            }
        }
        catch(Exception e)
        {         
            Debug.LogError(e.Message);   
            return;
        }

        Debug.Log("Load complete");
    }

    private string GetPathByOrientation()
    {
        ScreenOrientation type = CurrentOrientaion();
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

    private ScreenOrientation CurrentOrientaion()
    {
        ScreenOrientation type;

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

    private void CreateJsonDirectory()
    {
        string path = $"{Application.dataPath}/Resources/{GetPathByOrientation()}";
        if(!File.Exists(path))
            Directory.CreateDirectory(path);
    }    
}
