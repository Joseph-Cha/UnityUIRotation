using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class PropertyNameValuePair
{
    public PropertyNameValuePair(string key, string value)
    {
        this.Key = key;
        this.Value = value;
    }
     
    public string Key;
    public string Value;
}
