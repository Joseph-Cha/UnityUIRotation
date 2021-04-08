using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

enum ComponentType
{
    RectTransform,
    LayoutElement,
    VerticalLayoutGroup,
    GridLayoutGroup,
    TMP_Text,
    ScrollRect
}

public class ComponentProperty : MonoBehaviour
{
    public Transform Target;
    private List<Component> components = new List<Component>();
    private ScreenOrientationState ScreenOrientationState = new ScreenOrientationState();
    private void Awake()
    {
        if (Target == null)
            Target = GetComponent<Transform>();
    }
    private void OnEnable() => UIPropertyManager.Instance.OnLoadEventHandler += Load;

#if UNITY_EDITOR
    [MenuItem("ComponentProperty/Save #s")]
    static public void OnSaveMenu()
    {
        if(!Selection.activeGameObject)
            return;     
        var ComponentProperty = Selection.activeGameObject.GetComponent<ComponentProperty>();
        ComponentProperty.Save();
    }
#endif

    // [ContextMenu("Save")]
    public void Save()
    {
        CreateJsonDirectory();

        
        if(components == null || components.Count == 0)
        {
            components = 
                Target.GetComponentsInChildren<Component>(true).
                Where(component => component.CompareTag("UIProperty")).
                Where(component => SelectByComponentType(component)).ToList();
        }
        
        if (components != null)
            Save(components);
        else
            Debug.LogError("There are no Components tagged \"UIProperty\" in the target");        
    }

    private void Save(IEnumerable<Component> components)
    {
        // Arrage components data
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string path =  $"{Application.dataPath}/Resources/{currentOrientation/*GetPathByOrientation()*/}/{Target.name}.json";
        string jsonData = string.Empty;
        ComponentStore componentStore = new ComponentStore();
        // components.ToList().ForEach(component => componentStore.Data.Add(new ComponentInfo(component)));

        // Convert components data to json
        try
        {
            jsonData = JsonUtility.ToJson(componentStore, true);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            return;
        }
           
        // Save a json file to Resources folder
        File.WriteAllText(path, jsonData);
        #if UNITY_EDITOR
        var relativePath = $"Assets/Resources/{currentOrientation}/{Target.name}.json";
        AssetDatabase.ImportAsset(relativePath);
        #endif
        Debug.Log($"Save Complete.\nFile Location : {path}");
    }
    private bool SelectByComponentType(Component component)
    {
        Type componetType = component.GetType();
        string name = componetType?.Name;
        foreach(var type in Enum.GetValues(typeof(ComponentType)))
        {
            if (name.Equals(type?.ToString()))
                return true;
        }
        return false;
    }
    // [ContextMenu("Load")]    
    public void Load()
    {
        if(components == null)
        {
            components = 
                Target.GetComponentsInChildren<Component>(true).
                Where(component => component.CompareTag("UIProperty")).
                Where(component => SelectByComponentType(component)).ToList();
        }
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        TextAsset jsonFile;
        string resourcePath = $"{currentOrientation}/{Target.name}";
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
                // store.Data[i].SetPropertyValueByComponent(components[i]);
            }
        }
        catch(Exception e)
        {         
            Debug.LogError(e.Message);   
            return;
        }

        Debug.Log("Load complete");
    }

    private void CreateJsonDirectory()
    {
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string path = $"{Application.dataPath}/Resources/{currentOrientation}";
        if(!File.Exists(path))
            Directory.CreateDirectory(path);
    }
}
