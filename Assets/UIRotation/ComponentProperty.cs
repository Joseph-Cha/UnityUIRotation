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
    public Transform Root;
    private ScreenOrientationState ScreenOrientationState = new ScreenOrientationState();
    
    private void Awake()
    {
        if (Root == null)
            Root = GetComponent<Transform>();
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

    private void Save()
    {
        var node = new ComponentsNode(Root.name);
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string path =  $"{Application.dataPath}/Resources/{currentOrientation}/{Root.name}.json";
        string jsonData = string.Empty;
        
        Transform currentParent = Root;
        ComponentsNode currentNode = node;
        var childTransforms = new Queue<Transform>();
        var componentsNodes = new Queue<ComponentsNode>();

        while(true)
        {
            foreach(Transform child in currentParent)
            {
                SetComponentInfoToNode(currentNode, child);
                childTransforms.Enqueue(child);
            }
            foreach(ComponentsNode child in currentNode.Children)
            {
                componentsNodes.Enqueue(child);
            }
            if(childTransforms.Count == 0 || componentsNodes.Count == 0)
                break;
            currentParent = childTransforms.Dequeue();
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
        File.WriteAllText(path, jsonData);
        #if UNITY_EDITOR
        var relativePath = $"Assets/Resources/{currentOrientation}/{Root.name}.json";
        AssetDatabase.ImportAsset(relativePath);
        #endif
        Debug.Log($"Save Complete.\nFile Location : {path}");
    }

    private void SetComponentInfoToNode(ComponentsNode node, Transform target)
    {
        // 타겟의 이름으로 노드 생성
        ComponentsNode childNode = new ComponentsNode(target.name);
        
        // 노드에 타겟의 컴포넌트 정보 입력
        IEnumerable<Component> components = target.GetComponents<Component>().Where(component => SelectByComponentType(component));
        foreach(var component in components)
        {
            Type componetType = component.GetType();
            string name = componetType?.Name;
            childNode.ComponentInfos.Add(new ComponentInfo(name, component));
        }

        // 부모 노드에 타겟 노드를 연결
        node.Children.Add(childNode);
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
    [ContextMenu("Load")]    
    public void Load()
    {
        ComponentsNode node = null;
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        TextAsset jsonFile;
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

        Transform currentTransform = Root;
        ComponentsNode currentNode = node;
        var childTransforms = new Queue<Transform>();
        var componentsNodes = new Queue<ComponentsNode>();

        while(true)
        {
            foreach(Transform child in currentTransform)
            {
                childTransforms.Enqueue(child);
            }
            foreach(ComponentsNode child in currentNode.Children)
            {
                componentsNodes.Enqueue(child);
            }
            if(childTransforms.Count == 0 || componentsNodes.Count == 0)
                break;
            currentTransform = childTransforms.Dequeue();
            currentNode = componentsNodes.Dequeue();
            Test(currentNode, currentTransform);
        }
         

        Debug.Log("Load complete");
    }
    
    private void Test(ComponentsNode node, Transform target)
    {   
        int i = 0;
        IEnumerable<Component> components = target.GetComponents<Component>().Where(component => SelectByComponentType(component));
        foreach(var component in components)
        {
            node.ComponentInfos[i].SetPropertyValueByComponent(component);
            i++;
        }
    }


    private void CreateJsonDirectory()
    {
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string path = $"{Application.dataPath}/Resources/{currentOrientation}";
        if(!File.Exists(path))
            Directory.CreateDirectory(path);
    }
}
