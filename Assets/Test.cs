using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Test : MonoBehaviour
{
    [ContextMenu("test")]
    public void Test1() {
        float testValue = 10.0f;
        Debug.Log( JsonUtility.ToJson(testValue) );
        Debug.Log( testValue );
        string testValueString = testValue.ToString();
        Debug.Log( float.Parse(testValueString) );
        Assert.IsTrue(testValue == float.Parse(testValueString));
    }
}
