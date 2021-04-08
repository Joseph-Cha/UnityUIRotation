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
    private ScreenOrientationState ScreenOrientationState = new ScreenOrientationState();

    [ContextMenu("Test")]
    public void Test()
    {
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        string path =  $"{Application.dataPath}/Resources/{currentOrientation/*GetPathByOrientation()*/}/{Root.name}.json";

        ComponentsTreeNode Node = new ComponentsTreeNode(Root.name);

        IEnumerable<Component> coms = Root.GetComponents<Component>().Where(component => SelectByComponentType(component));
        foreach(var component in coms)
        {
            Type componetType = component.GetType();
            string name = componetType?.Name;
            Node.ComponentInfos.Add(new ComponentInfo(name, component));
        }

        
        SetTreeNode(Root, Node);

        // // current를 그때 그때 수정하자
        // // https://en.wikipedia.org/wiki/Search_tree#Iterative
        // IEnumerator list = Root.GetEnumerator();
        // while(list.MoveNext())
        // {
        //     Transform transform = list.Current as Transform;
        //     Node.Children.Add(new ComponentsTreeNode(transform.name));
        //     foreach(var node in Node.Children)
        //     {
        //         IEnumerable<Component> components = transform.GetComponents<Component>().
        //             Where(component => SelectByComponentType(component));
        //         foreach(var component in components)
        //         {
        //             Type componetType = component.GetType();
        //             string name = componetType?.Name;
        //             node.ComponentInfos.Add(new ComponentInfo(name, component));
        //         }
        //     }
        // }
        string jsonData = JsonUtility.ToJson(Node, true);

        File.WriteAllText(path, jsonData);
        #if UNITY_EDITOR
        var relativePath = $"Assets/Resources/{currentOrientation}/{Root.name}.json";
        AssetDatabase.ImportAsset(relativePath);
        #endif
        Debug.Log($"Save Complete.\nFile Location : {path}");

    }
    private void SetTreeNode(Transform _root, ComponentsTreeNode _node)
    {
        Transform currentTransform = _root;
        while(currentTransform.childCount != 0)
        {
            // IEnumerable<Component> coms = currentTransform.GetComponents<Component>().Where(component => SelectByComponentType(component));
            // foreach(var component in coms)
            // {
            //     Type componetType = component.GetType();
            //     string name = componetType?.Name;
            //     _node.ComponentInfos.Add(new ComponentInfo(name, component));
            // }

            // currentTransform = 

            IEnumerator list = currentTransform.GetEnumerator();
            while(list.MoveNext())
            {
                currentTransform = list.Current as Transform;

                
                _node.Children.Add(new ComponentsTreeNode(currentTransform.name));

                foreach(var node in _node.Children)
                {
                    IEnumerable<Component> components = currentTransform.GetComponents<Component>().Where(component => SelectByComponentType(component));
                    foreach(var component in components)
                    {
                        Type componetType = component.GetType();
                        string name = componetType?.Name;
                        node.ComponentInfos.Add(new ComponentInfo(name, component));
                    }
                }
            }

            // IEnumerable<Component> coms = currentTransform.GetComponents<Component>().Where(component => SelectByComponentType(component));
            // foreach(var component in coms)
            // {
            //     Type componetType = component.GetType();
            //     string name = componetType?.Name;
            //     _node.ComponentInfos.Add(new ComponentInfo(name, component));
            // }
        }


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
