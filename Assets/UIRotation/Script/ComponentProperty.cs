using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.UI;

public class ComponentProperty : MonoBehaviour
{
    public Transform Root;
    private ComponentsNode portraitNode = null;
    private ComponentsNode landscapeNode = null;
    private ScreenOrientationState ScreenOrientationState = new ScreenOrientationState();
    private ScreenOrientation currentOrientationType;
    private string IgnoreTag = "Ignore";
    private void Awake()
    {
        if(Root == null)
            Root = this.transform;
    #if !UNITY_EDITOR
        portraitNode = GetNodeByCurrentOrientation(ScreenOrientation.Portrait);
        landscapeNode = GetNodeByCurrentOrientation(ScreenOrientation.Landscape);
    #endif
    }
    private void OnEnable()
    {
        ComponentsManager.Instance.ScreenRotated += Load;
        Load();
    }
    private void OnDisable() => ComponentsManager.Instance.ScreenRotated -= Load;

#if UNITY_EDITOR
    [MenuItem("ComponentProperty/Save #s")]
    static public void OnSaveMenu()
    {
        if(!Selection.activeGameObject)
            return;     
        var ComponentProperty = Selection.activeGameObject.GetComponent<ComponentProperty>();
        ComponentProperty.Save();
    }
    [MenuItem("ComponentProperty/Load #l")]
    static public void OnLoadMenu()
    {
        if(!Selection.activeGameObject)
            return;     
        var ComponentProperty = Selection.activeGameObject.GetComponent<ComponentProperty>();
        ComponentProperty.Load();
    }
#endif

    private void Save()
    {
        string name = Root.name;
        var node = new ComponentsNode(Root.name);
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string path =  $"{Application.dataPath}/Resources/{currentOrientation}/{SceneManager.GetActiveScene().name}/{Root.name}.json";
        string jsonData = string.Empty;
        node = AddComponentInfo(Root);
        TreeSearch(Root, node, GetChildNodeForSave);
        // Convert components data to json
        try
        {
            jsonData = JsonConvert.SerializeObject(node, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                Formatting = Formatting.Indented
            });
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            return;
        }
        // Save a json file to Resources folder
        CreateDirectory();
        File.WriteAllText(path, jsonData);
    #if UNITY_EDITOR
        var relativePath = $"Assets/Resources/{currentOrientation}/{SceneManager.GetActiveScene().name}/{Root.name}.json";
        AssetDatabase.ImportAsset(relativePath);
    #endif
        Debug.Log($"Save Complete.\nFile Location : {path}");
    }
    
    public void Load()
    {
        ScreenOrientation type = ScreenOrientationState.CurrentOrientaion();
        switch(type)
        {
            case ScreenOrientation.Portrait:
            #if UNITY_EDITOR
                this.portraitNode = GetNodeByCurrentOrientation(type);
            #endif
                SetComponentInfo(Root, this.portraitNode);
                TreeSearch(Root, this.portraitNode, GetChildNodeForLoad);
                break;
            case ScreenOrientation.LandscapeLeft:
            case ScreenOrientation.LandscapeRight:
            #if UNITY_EDITOR
                this.landscapeNode = GetNodeByCurrentOrientation(type);
            #endif
                SetComponentInfo(Root, this.landscapeNode);
                TreeSearch(Root, this.landscapeNode, GetChildNodeForLoad);
                break;
        }
        LayoutGroupUpdate();
        Debug.Log($"Load perfectly. Current Orientaion : {type} / Target name : {transform.name}");
    }    

    private void TreeSearch(Transform root, ComponentsNode node, Func<Transform, ComponentsNode, ComponentsNode> callBackForGettingChildNode)
    {
        Transform currentTransform = root;
        ComponentsNode currentNode = node;
        var transforms = new Queue<(Transform, ComponentsNode)>();
        while(true)
        {
            foreach (Transform childTransform in currentTransform)
            { 
                // IgnoreTag가 달린 GameObject는 탐색을 건너뜀
                if (childTransform.tag == IgnoreTag)
                {
                    continue;
                }

                ComponentsNode childNode = callBackForGettingChildNode(childTransform, currentNode);
                if(childNode != null)
                    transforms.Enqueue((childTransform, childNode));
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
        // 타겟의 이름으로 노드 생성
        var node = new ComponentsNode(childTransform.name);
        
        // 노드에 타겟의 컴포넌트 정보 입력
        var components = childTransform.GetComponents<Component>();
        foreach(var component in components)
        {
            Type componetType = component.GetType();
            string name = componetType.Name;
            var componentInfo = new ComponentInfo(name, component);
            if (componentInfo.IsExistProperties)
            {
                node.ComponentInfos.Add(componentInfo);
            }
        }
        // 해당 노드를 반환
        return node;
    }

    private ComponentsNode GetChildNodeForLoad(Transform child, ComponentsNode node)
    {
        ComponentsNode childNode = node?.Children.Find(node => node.Name == child.name);
        if(childNode != null)
            SetComponentInfo(child, childNode);
        return childNode;
    }

    private void SetComponentInfo(Transform transform, ComponentsNode node)
    {
        var components = transform.GetComponents<Component>();
        foreach(var component in components)
        {
            Type componetType = component.GetType();
            string name = componetType.Name;
            var componentInfo = node?.ComponentInfos.Find(info => info.Name == name);
            componentInfo?.SetPropertyValueByComponent(component);
        }
    }

    private void CreateDirectory()
    {
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string path = $"{Application.dataPath}/Resources/{currentOrientation}/{SceneManager.GetActiveScene().name}";
        if(!File.Exists(path))
            Directory.CreateDirectory(path);
    }

    private ComponentsNode GetNodeByCurrentOrientation(ScreenOrientation type)
    {
        ComponentsNode node = null;
        string name = Root.name;
        string toRemove = "(Clone)";
        if(name.Contains(toRemove))
        {
            int i = Root.name.IndexOf(toRemove);
            if(i >= 0)
                name = Root.name.Remove(i, toRemove.Length);
        }
        string currentPath =  ScreenOrientationState.GetPathByOrientation(type);
        string resourcePath = $"{currentPath}/{SceneManager.GetActiveScene().name}/{name}";        
        TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);

        if(jsonFile == null)
        {
            Debug.LogWarning("It couldn't find saved file. You must save first and try again");
            return null;
        }

        try
        {
            node = JsonConvert.DeserializeObject<ComponentsNode>(jsonFile.text, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            });
        }
        catch(Exception e)
        {         
            Debug.LogError(e.Message);   
            return null;
        }
        return node;
    }

    private void LayoutGroupUpdate()
    {
        var layoutGroups = Root.GetComponentsInChildren<LayoutGroup>();
        foreach (var layoutGroup in layoutGroups)
        {
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();        
        }
    }
}
