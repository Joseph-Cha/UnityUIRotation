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

        IEnumerable<Component> Components = Target.GetComponentsInChildren<Component>(true)?.Where(component => component.CompareTag("UIProperty"));
        // IEnumerable<Component> Components2 = GameObject.FindGameObjectsWithTag("UIProperty").Select(obj => obj.GetComponent<Component>());
        if (Components != null)
            Save(Components);
        else
            Debug.Log("There are no Components tagged \"UIProperty\" in the target");
    }

    private void Save(IEnumerable<Component> Components)
    {
        // Arrage components data
        string path =  $"{Application.dataPath}/Resources/{GetPathByOrientation()}/{name}.json";
        string jsonData = string.Empty;
        ComponentStore StoreInfo = new ComponentStore();
        Components.ToList().ForEach(Component => StoreInfo.Data.Add(new ComponentInfo(Component)));

        // Convert components data to json
        try
        {
            jsonData = JsonConvert.SerializeObject(StoreInfo, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
        
        // Save a json file to Resources folder
         if(File.Exists(path))
            File.Delete(path);
        File.WriteAllText(path, jsonData);

        #if UNITY_EDITOR
        AssetDatabase.Refresh();
        #endif
        Debug.Log($"Save Complete. (Current Orientaion : {CurrentOrientaion()})");       

    }

    public void Load()
    {
        TextAsset jsonFile;
        string resourcePath = GetPathByOrientation();
        
        try
        {
            jsonFile = Resources.Load<TextAsset>(resourcePath);
        }
        catch
        {            
            Debug.Log("Fail to load file. Please check if the file exist");
            return;
        }

        if(jsonFile != null)
        {
            var store = JsonConvert.DeserializeObject<ComponentStore>(jsonFile.text);
            StartCoroutine(Load(store));
        }

    }

    private IEnumerator Load(ComponentStore store)
    {
        foreach(var ComponentInfo in store.Data) 
        {
            
            yield return null;
        }
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
