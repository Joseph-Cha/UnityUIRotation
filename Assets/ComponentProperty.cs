using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComponentProperty : MonoBehaviour
{
    // Component를 가지고 와서 저장을 한다.
    // 저장한 Component를 불러온다.
    [ContextMenu("Save")]
    public void Save()
    {
        List<ComponentInfo> infos = new List<ComponentInfo>();
        List<Component[]> components = GameObject.FindGameObjectsWithTag("UIProperty").Select(obj => obj.GetComponents<Component>()).ToList();
        components.ForEach(component => infos.Add(new ComponentInfo(component)));
        var json = JsonUtility.ToJson(infos);
        Debug.Log(json);
    }




}
