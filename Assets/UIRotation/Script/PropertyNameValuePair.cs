using System;

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
