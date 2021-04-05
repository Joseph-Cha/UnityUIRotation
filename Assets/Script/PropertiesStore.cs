using System;
using System.Collections.Generic;

[Serializable]
public class PropertiesStore
{
    public List<string> Key = new List<string>();
    public List<string> Value = new List<string>();
    public void Add(string key, string value)
    {
        Key.Add(key);
        Value.Add(value);
    }

    public string GetValueByKey(string key)
    {
        string value = null;
        for (int i = 0; i < Key.Count; i++)
        {
            if(Key[i].Equals(key))
            {
                value = Value[i];
                break;
            }
        }
        return value;
    }
}