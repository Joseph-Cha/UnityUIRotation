using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;

public class TestClass : MonoBehaviour
{
    public Transform Root;
    private ComponentsNode Node = null;
    private ScreenOrientationState ScreenOrientationState = new ScreenOrientationState();

    [ContextMenu("Test")]
    public void Test()
    {
        Node = new ComponentsNode(Root.name);
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string path =  $"{Application.dataPath}/Resources/{currentOrientation}/{Root.name}.json";

        Transform currentParent = Root;
        ComponentsNode currentNode = Node;
        Queue<Transform> childTransforms = new Queue<Transform>();
        Queue<ComponentsNode> ComponentsNodes = new Queue<ComponentsNode>();
        while(true)
        {
            foreach(Transform child in currentParent)
            {
                SetComponentInfoToNode(currentNode, child);
                childTransforms.Enqueue(child);
            }
            foreach(ComponentsNode child in currentNode.Children)
            {
                ComponentsNodes.Enqueue(child);
            }
            
            // try
            // {
            //     currentParent = childTransforms.Dequeue();
            //     currentNode = ComponentsNodes.Dequeue();
            // }
            // catch
            // {
            //     break;
            // }
            if(childTransforms.Count != 0 || ComponentsNodes.Count != 0)
            {
                currentParent = childTransforms.Dequeue();
                currentNode = ComponentsNodes.Dequeue();
            }
            else
                break;
        }

        string jsonData = JsonUtility.ToJson(Node, true);

        File.WriteAllText(path, jsonData);
        #if UNITY_EDITOR
        var relativePath = $"Assets/Resources/{currentOrientation}/{Root.name}.json";
        AssetDatabase.ImportAsset(relativePath);
        #endif
        Debug.Log($"Save Complete.\nFile Location : {path}");
    }


    private void SetComponentInfoToNode(ComponentsNode _node, Transform target)
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
        _node.Children.Add(childNode);
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
}
