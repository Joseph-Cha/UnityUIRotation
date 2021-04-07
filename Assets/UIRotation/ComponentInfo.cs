using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.Reflection;
using System.Collections.Generic;

[Serializable]
public class PropertyKeyValuePair
{
    public PropertyKeyValuePair(string key, string value)
    {
        Key = key;
        Value = value;
    }
    public string Key { get; set; }
    public string Value { get; set; }
}

// 하나의 Root Transform에서 가지고 있는 components들의 속성 정보를 저장
[Serializable]
public class ComponentInfo
{
#region ComponentInfo ctor
    public ComponentInfo(Component component)
    {
        switch(component)
        {
            case RectTransform rectTransform:
                AddSerializeData(rectTransformPropertyNames, rectTransform);
                // RectTransformInfos = rectTransform.GetType().GetProperties();
                // RectTransformInfos.ToList().ForEach(info => Debug.Log($"RectTransformInfo Name : {info.Name}"));
                break;

            case LayoutElement layoutElement:
                AddSerializeData(layoutElementPropertyNames, layoutElement);
                // LayoutElementInfos = layoutElement.GetType().GetProperties();
                // LayoutElementInfos.ToList().ForEach(info => Debug.Log($"LayoutElement Info Name : {info.Name}"));
                break;

            case VerticalLayoutGroup verticalLayoutGroup:
                AddSerializeData(verticalLayoutGroupPropertyNames, verticalLayoutGroup);
                // VerticalLayoutGroupInfos = verticalLayoutGroup.GetType().GetProperties();
                // VerticalLayoutGroupInfos.ToList().ForEach(info => Debug.Log($"VerticalLayoutGroup Info Name : {info.Name}"));
                break;

            case GridLayoutGroup gridLayoutGroup:
                AddSerializeData(gridLayoutGroupPropertyNames, gridLayoutGroup);
                // GridLayoutGroupInfos = gridLayoutGroup.GetType().GetProperties();
                // GridLayoutGroupInfos.ToList().ForEach(info => Debug.Log($"GridLayoutGroupInfos Name : {info.Name}"));
                break;

            case TMP_Text text:
                AddSerializeData(tmp_TextPropertyNames, text);
                // TMPTextInfos = text.GetType().GetProperties();
                // TMPTextInfos.ToList().ForEach(info => Debug.Log($"TMPTextInfos Name : {info.Name}"));
                break;

            case ScrollRect scrollRect:
                AddSerializeData(scrollRectPropertyNames, scrollRect);
                // ScrollRectInfos = scrollRect.GetType().GetProperties();
                // ScrollRectInfos.ToList().ForEach(info => Debug.Log($"ScrollRectInfos Name : {info.Name}"));
                break;

            default:
                break;
        }
    }
#endregion
    public List<PropertyKeyValuePair> Properties = new List<PropertyKeyValuePair>();
    private PropertyInfo GetPropertyInfo(string name, object target)
    {
        Type type = target?.GetType();
        return type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
    }
#region PropertyNames
    private string[] rectTransformPropertyNames = 
    { 
        nameof(RectTransform.sizeDelta),
        nameof(RectTransform.pivot), 
        nameof(RectTransform.anchorMin),
        nameof(RectTransform.anchorMax),
        nameof(RectTransform.localScale),
        nameof(RectTransform.localPosition),
        nameof(RectTransform.localRotation),
        nameof(RectTransform.anchoredPosition)  
    };
    private string[] layoutElementPropertyNames =
    {
        nameof(LayoutElement.minWidth),
        nameof(LayoutElement.minHeight),
        nameof(LayoutElement.preferredWidth),
        nameof(LayoutElement.preferredHeight),
        nameof(LayoutElement.flexibleWidth),
        nameof(LayoutElement.flexibleHeight),
    };
    private string[] verticalLayoutGroupPropertyNames =
    {
        nameof(VerticalLayoutGroup.spacing),
        nameof(VerticalLayoutGroup.childForceExpandWidth),
        nameof(VerticalLayoutGroup.childForceExpandHeight),
        nameof(VerticalLayoutGroup.childControlWidth),
        nameof(VerticalLayoutGroup.childControlHeight),
        nameof(VerticalLayoutGroup.childScaleWidth),
        nameof(VerticalLayoutGroup.childScaleHeight),
        nameof(VerticalLayoutGroup.reverseArrangement),
        nameof(VerticalLayoutGroup.padding),
        nameof(VerticalLayoutGroup.childAlignment)
    };
    private string[] gridLayoutGroupPropertyNames =
    {
        nameof(GridLayoutGroup.startCorner),
        nameof(GridLayoutGroup.startAxis),
        nameof(GridLayoutGroup.cellSize),
        nameof(GridLayoutGroup.spacing),
        nameof(GridLayoutGroup.constraint),
        nameof(GridLayoutGroup.constraintCount),
        nameof(GridLayoutGroup.padding),
        nameof(GridLayoutGroup.childAlignment)
    };
    private string[] tmp_TextPropertyNames = 
    {
        nameof(TMP_Text.alignment),
        nameof(TMP_Text.horizontalAlignment),
        nameof(TMP_Text.verticalAlignment),
        nameof(TMP_Text.autoSizeTextContainer),
        nameof(TMP_Text.fontSize),
        nameof(TMP_Text.fontSizeMin),
        nameof(TMP_Text.fontSizeMax),
        nameof(TMP_Text.characterSpacing),
        nameof(TMP_Text.wordSpacing),
        nameof(TMP_Text.lineSpacing),
        nameof(TMP_Text.lineSpacingAdjustment),
        nameof(TMP_Text.paragraphSpacing),
        nameof(TMP_Text.textStyle),
        nameof(TMP_Text.fontStyle)
    };
    private string[] scrollRectPropertyNames = 
    {
        nameof(ScrollRect.horizontal),
        nameof(ScrollRect.vertical),
        nameof(ScrollRect.movementType),
        nameof(ScrollRect.elasticity),
        nameof(ScrollRect.inertia),
        nameof(ScrollRect.decelerationRate),
        nameof(ScrollRect.scrollSensitivity),
        nameof(ScrollRect.horizontalScrollbarVisibility),
        nameof(ScrollRect.verticalScrollbarVisibility),
        nameof(ScrollRect.horizontalScrollbarSpacing),
        nameof(ScrollRect.verticalScrollbarSpacing),
    };
#endregion    
    
#region Save Logic
    private void AddSerializeData(string[] propertyNames, object target)
    {
        if(propertyNames != null || target != null)
        {
            foreach(var propertyName in propertyNames)
            {
                try
                {
                    PropertyInfo info = GetPropertyInfo(propertyName, target);
                    if(info.PropertyType.IsPrimitive)
                    {
                        Properties.Add(new PropertyKeyValuePair(
                            propertyName, GetValueByPropertyName(propertyName, target).ToString()));
                    }
                    else
                    {
                        Properties.Add(new PropertyKeyValuePair(
                            propertyName, JsonUtility.ToJson(GetValueByPropertyName(propertyName, target))));
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError(e.Message);
                    continue;
                }
            }
        }
    }
    private object GetValueByPropertyName(string name, object target)
    {
        Type type = target?.GetType();
        PropertyInfo info = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
        return info?.GetValue(target);
    }
#endregion

#region Load Logic
    public void SetPropertyValueByComponent(Component component)
    {
        switch(component)
        {
            case RectTransform rectTransform:
            case LayoutElement layoutElement:
            case VerticalLayoutGroup verticalLayoutGroup:
            case GridLayoutGroup gridLayoutGroup:
            case TMP_Text text:
            case ScrollRect scrollRect:
                SetValueByTarget(component);
                break;
            default:
                break;
        }
    }
    private void SetValueByTarget(object target)
    {
        foreach(var property in Properties)
        {
            PropertyInfo info = GetPropertyInfo(property.Key, target);
            Type PropertyType = info?.PropertyType;
            object obj = null;
            if(PropertyType.IsPrimitive)
                obj = Convert.ChangeType(property.Value, PropertyType);
            else
                obj = JsonUtility.FromJson(property.Value, PropertyType);

            info.SetValue(target, obj);
        }
    }
#endregion

    // 향후 componet의 Property 값을 추가 하기 위해 삭제 금지
    // private PropertyInfo[] RectTransformInfos;
    // private PropertyInfo[] LayoutElementInfos;
    // private PropertyInfo[] VerticalLayoutGroupInfos;
    // private PropertyInfo[] GridLayoutGroupInfos;
    // private PropertyInfo[] TMPTextInfos;
    // private PropertyInfo[] TextInfos;
    // private PropertyInfo[] ScrollRectInfos;
}
