using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

// 하나의 Root Transform에서 가지고 있는 components들의 속성 정보를 저장
[Serializable]
public class ComponentInfo
{
#region  ComponentInfo ctor
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
    public Dictionary<string, string> ComponentInfos = new Dictionary<string, string>();
    private JsonSerializerSettings JsonSetting = new JsonSerializerSettings()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

#region  PropertyNames
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
        nameof(VerticalLayoutGroup.childAlignment),
        nameof(VerticalLayoutGroup.minWidth),
        nameof(VerticalLayoutGroup.preferredWidth),
        nameof(VerticalLayoutGroup.flexibleWidth),
        nameof(VerticalLayoutGroup.minHeight),
        nameof(VerticalLayoutGroup.preferredHeight),
        nameof(VerticalLayoutGroup.flexibleHeight)
    };
    private string[] gridLayoutGroupPropertyNames;
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
        nameof(ScrollRect.horizontalScrollbar),
        nameof(ScrollRect.verticalScrollbar),
        nameof(ScrollRect.horizontalScrollbarVisibility),
        nameof(ScrollRect.verticalScrollbarVisibility),
        nameof(ScrollRect.horizontalScrollbarSpacing),
        nameof(ScrollRect.verticalScrollbarSpacing),
        nameof(ScrollRect.onValueChanged),
        nameof(ScrollRect.velocity),
        nameof(ScrollRect.normalizedPosition),
        nameof(ScrollRect.horizontalNormalizedPosition),
        nameof(ScrollRect.verticalNormalizedPosition),
        nameof(ScrollRect.minWidth),
        nameof(ScrollRect.preferredWidth),
        nameof(ScrollRect.flexibleWidth),
        nameof(ScrollRect.minHeight),
        nameof(ScrollRect.preferredHeight),
        nameof(ScrollRect.flexibleHeight)
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
                    ComponentInfos.Add(propertyName, 
                        JsonConvert.SerializeObject(GetValueByPropertyName(propertyName, target), JsonSetting));
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
        PropertyInfo info = type.GetProperty(name);
        return info?.GetValue(target);
    }
#endregion

#region Load Logic
    private void Apply(Component component)
    {
        switch(component)
        {
            case RectTransform rectTransform:
                SetValueByPropertyName(rectTransformPropertyNames, rectTransform);
                break;
            case LayoutElement layoutElement:
                SetValueByPropertyName(layoutElementPropertyNames, layoutElement);
                break;
            case VerticalLayoutGroup verticalLayoutGroup:
                SetValueByPropertyName(verticalLayoutGroupPropertyNames, verticalLayoutGroup);
                break;
            case GridLayoutGroup gridLayoutGroup:
                SetValueByPropertyName(gridLayoutGroupPropertyNames, gridLayoutGroup);
                break;
            case TMP_Text text:
                SetValueByPropertyName(tmp_TextPropertyNames, text);
                break;
            case ScrollRect scrollRect:
                SetValueByPropertyName(scrollRectPropertyNames, scrollRect);
                break;
            default:
                break;
        }
    }

    public void Apply(IEnumerable<Component> components)
    {
        var enumerator = components.GetEnumerator();
        while (enumerator.MoveNext())
        {
            Apply(enumerator.Current);
        }
    }

    private void SetValueByPropertyName(string[] PropertyNames, object target)
    {
        foreach (var name in PropertyNames)
        {
            Type type = target?.GetType();
            PropertyInfo info = type.GetProperty(name);
            info.SetValue(target, GetDeserializeValuebyName(name));
        }
    }

    private object GetDeserializeValuebyName(string name)
    {
        object ob = JsonConvert.DeserializeObject<object>(ComponentInfos[name]);
        return ob;
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
