using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ComponentProperty : MonoBehaviour
{
    public Transform Root;
    private ScreenOrientationState ScreenOrientationState = new ScreenOrientationState();
    private void Awake()
    {
        if (Root == null)
            Root = transform;
    }

    private void OnEnable() => ComponentsManager.Instance.OnLoadEventHandler += Load;

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

    private void Save()
    {
        var node = new ComponentsNode(Root.name);
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string path =  $"{Application.dataPath}/Resources/{currentOrientation}/{SceneManager.GetActiveScene().name}/{Root.name}.json";
        string jsonData = string.Empty;
        
        Transform currentRoot = Root;
        ComponentsNode currentNode = node;
        var transforms = new Queue<Transform>();
        var componentsNodes = new Queue<ComponentsNode>();

        while(true)
        {
            foreach(Transform childTransform in currentRoot)
            {
                ComponentsNode childNode = AddComponentInfo(childTransform);
                currentNode.Children.Add(childNode);
                transforms.Enqueue(childTransform);
                componentsNodes.Enqueue(childNode);
            }
            if(transforms.Count == 0)
                break;
            currentRoot = transforms.Dequeue();
            currentNode = componentsNodes.Dequeue();
        }

        // Convert components data to json
        try
        {
            jsonData = JsonUtility.ToJson(node, true);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            return;
        }
        
        // Save a json file to Resources folder
        CreateJsonDirectory();
        File.WriteAllText(path, jsonData);
        #if UNITY_EDITOR
        var relativePath = $"Assets/Resources/{currentOrientation}/{SceneManager.GetActiveScene().name}/{Root.name}.json";
        AssetDatabase.ImportAsset(relativePath);
        #endif
        Debug.Log($"Save Complete.\nFile Location : {path}");
    }

    private ComponentsNode AddComponentInfo(Transform childTransform)
    {
        // 타겟의 이름으로 자식 노드 생성
        var childNode = new ComponentsNode(childTransform.name);
        
        // 노드에 타겟의 컴포넌트 정보 입력
        var components = childTransform.GetComponents<Component>();
        foreach(var component in components)
        {
            Type componetType = component.GetType();
            string name = componetType?.Name;
            var componentInfo = new ComponentInfo(name, component);
            if (componentInfo.IsExistProperties)
            {
                childNode.ComponentInfos.Add(componentInfo);
            }
        }
        // 부모 노드에 붙이기 위해서 자식 노드를 반환
        return childNode;
    }

    [ContextMenu("Load")]    
    public void Load()
    {
        TextAsset jsonFile;
        ComponentsNode node = null;
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string resourcePath = $"{currentOrientation}/{Root.name}";
        jsonFile = Resources.Load<TextAsset>(resourcePath);

        if(jsonFile == null)
        {
            Debug.LogError("It couldn't find saved file. You must save first and try again");
            return;
        }

        try
        {
            node = JsonUtility.FromJson<ComponentsNode>(jsonFile.text);
        }
        catch(Exception e)
        {         
            Debug.LogError(e.Message);   
            return;
        }

        Transform currentRoot = Root;
        ComponentsNode currentNode = node;
        var transforms = new Queue<Transform>();
        var componentsNodes = new Queue<ComponentsNode>();        
        while(true)
        {
            foreach(Transform childTransform in Root)
            {   
                if(childTransform.tag.Contains("dynamic"))
                    continue;
                 SetComponentInfo(childTransform, currentNode);
                transforms.Enqueue(childTransform);
                // componentsNodes.Enqueue(childNode);
            }
            if(transforms.Count == 0)
                break;
            currentRoot = transforms.Dequeue();
            currentNode = componentsNodes.Dequeue();
        }

        Debug.Log("Load perfectly");
    }
    private void SetComponentInfo(Transform childTransform, ComponentsNode parentNode)
    {
        
        var components = childTransform.GetComponents<Component>();
        int i = 0;
        foreach(var component in components)
        {
            parentNode.ComponentInfos[i].SetPropertyValueByComponent(component);
            i++;
        }
    }
    private void CreateJsonDirectory()
    {
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string path = $"{Application.dataPath}/Resources/{currentOrientation}/{SceneManager.GetActiveScene().name}";
        if(!File.Exists(path))
            Directory.CreateDirectory(path);
    }
}