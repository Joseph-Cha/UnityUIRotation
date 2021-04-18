using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class ComponentProperty : MonoBehaviour
{
    public Transform Root;
    private ScreenOrientationState ScreenOrientationState = new ScreenOrientationState();
    private string tagName = "Dynamic";
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

        TreeSearch(Root, node, GetChildNodeForSave);
        // Convert components data to json
        try
        {
            jsonData = JsonConvert.SerializeObject(node, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
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
    
    [ContextMenu("Load")]    
    public void Load()
    {
        TextAsset jsonFile;
        ComponentsNode node = null;
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string resourcePath = $"{currentOrientation}/{SceneManager.GetActiveScene().name}/{Root.name}";
        jsonFile = Resources.Load<TextAsset>(resourcePath);

        if(jsonFile == null)
        {
            Debug.LogError("It couldn't find saved file. You must save first and try again");
            return;
        }

        try
        {
            node = JsonConvert.DeserializeObject<ComponentsNode>(jsonFile.text, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        catch(Exception e)
        {         
            Debug.LogError(e.Message);   
            return;
        }

        TreeSearch(Root, node, GetChildNodeForLoad);

        Debug.Log("Load perfectly");
    }    
    private void TreeSearch(Transform root, ComponentsNode node, Func<Transform, ComponentsNode, ComponentsNode> GetChildNode)
    {
        Transform currentTransform = Root;
        ComponentsNode currentNode = node;
        var transforms = new Queue<(Transform, ComponentsNode)>();
        while(true)
        {
            foreach (Transform childTransform in currentTransform)
            {
                if(!childTransform.CompareTag(tagName))
                {
                    ComponentsNode childNode = GetChildNode(childTransform, currentNode);
                    transforms.Enqueue((childTransform, childNode));
                }
            }
            if(transforms.Count == 0)
                break;
            currentTransform = transforms.Peek().Item1;
            currentNode = transforms.Dequeue().Item2;
        }
    } 
    private ComponentsNode GetChildNodeForSave(Transform child, ComponentsNode node)
    {
        ComponentsNode childNode = AddComponentInfo(child);
        node.Children.Add(childNode);
        return childNode;
    }
    private ComponentsNode AddComponentInfo(Transform childTransform)
    {
        // 타겟의 이름으로 자식 노드 생성
        var childNode = new ComponentsNode(childTransform.name);
        
        // 노드에 타겟의 컴포넌트 정보 입력
        var components = childTransform.GetComponents<Component>();
        foreach(var component in components)
        {
            Type componetType = component?.GetType();
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
    private ComponentsNode GetChildNodeForLoad(Transform child, ComponentsNode node)
    {
        ComponentsNode childNode = node.Children.Find(node => node.Name == child.name);
        SetComponentInfo(child, childNode);
        return childNode;
    }
    private void SetComponentInfo(Transform childTransform, ComponentsNode childNode)
    {
        var components = childTransform.GetComponents<Component>();
        foreach(var component in components)
        {
            Type componetType = component.GetType();
            string name = componetType.Name;
            childNode?.ComponentInfos.Find(info => info.Name.Contains(name))?.SetPropertyValueByComponent(component);
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
