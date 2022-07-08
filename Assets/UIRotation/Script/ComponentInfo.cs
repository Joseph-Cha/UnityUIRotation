using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 하나의 Root Transform에서 가지고 있는 components들의 속성 정보를 저장
[Serializable]
public class ComponentInfo
{
    public string Name;
    public List<PropertyNameValuePair> Properties = new List<PropertyNameValuePair>();
    public bool IsExistProperties => Properties.Count != 0;
    private ComponentInfo(string name) => this.Name = name;
    public ComponentInfo(string name, Component component) : this(name) => AddSerializeData(component);
    private PropertyInfo GetPropertyInfo(string name, object target)
    {
        Type type = target?.GetType();
        return type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
    }

    #region PropertyNames
    private Dictionary<Type, string[]> propertyNames = new Dictionary<Type, string[]>()
    {
        [typeof(RectTransform)] = new string[]
        {
            nameof(RectTransform.sizeDelta),
            nameof(RectTransform.pivot),
            nameof(RectTransform.anchorMin),
            nameof(RectTransform.anchorMax),
            nameof(RectTransform.localScale),
            nameof(RectTransform.localPosition),
            nameof(RectTransform.localRotation),
            nameof(RectTransform.anchoredPosition)
        },
        [typeof(LayoutElement)] = new string[]
        {
            nameof(LayoutElement.minWidth),
            nameof(LayoutElement.minHeight),
            nameof(LayoutElement.preferredWidth),
            nameof(LayoutElement.preferredHeight),
            nameof(LayoutElement.flexibleWidth),
            nameof(LayoutElement.flexibleHeight),
            nameof(LayoutElement.ignoreLayout)
        },
        [typeof(GridLayoutGroup)] = new string[]
        {
            nameof(GridLayoutGroup.startCorner),
            nameof(GridLayoutGroup.startAxis),
            nameof(GridLayoutGroup.cellSize),
            nameof(GridLayoutGroup.spacing),
            nameof(GridLayoutGroup.constraint),
            nameof(GridLayoutGroup.constraintCount),
            nameof(GridLayoutGroup.childAlignment)
        },
        [typeof(VerticalLayoutGroup)] = new string[]
        {
            nameof(VerticalLayoutGroup.spacing),
            nameof(VerticalLayoutGroup.childForceExpandWidth),
            nameof(VerticalLayoutGroup.childForceExpandHeight),
            nameof(VerticalLayoutGroup.childControlWidth),
            nameof(VerticalLayoutGroup.childControlHeight),
            nameof(VerticalLayoutGroup.childScaleWidth),
            nameof(VerticalLayoutGroup.childScaleHeight),
            nameof(VerticalLayoutGroup.reverseArrangement),
            nameof(VerticalLayoutGroup.childAlignment)
        },
        [typeof(HorizontalLayoutGroup)] = new string[]
        {
            nameof(HorizontalLayoutGroup.spacing),
            nameof(HorizontalLayoutGroup.childForceExpandWidth),
            nameof(HorizontalLayoutGroup.childForceExpandHeight),
            nameof(HorizontalLayoutGroup.childControlWidth),
            nameof(HorizontalLayoutGroup.childControlHeight),
            nameof(HorizontalLayoutGroup.childScaleWidth),
            nameof(HorizontalLayoutGroup.childScaleHeight),
            nameof(HorizontalLayoutGroup.reverseArrangement),
            nameof(HorizontalLayoutGroup.childAlignment)
        },
        [typeof(TMP_Text)] = new string[]
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
        },
        [typeof(ScrollRect)] = new string[]
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
            nameof(ScrollRect.verticalScrollbarSpacing)
        },
        [typeof(ContentSizeFitter)] = new string[]
        {
            nameof(ContentSizeFitter.horizontalFit),
            nameof(ContentSizeFitter.verticalFit)
        }
    };
    #endregion

    #region Save Logic
    private void AddSerializeData(object target)
    {
        var ComponentTypes = propertyNames.Keys.ToList();
        var type = ComponentTypes.Find(x => x.IsInstanceOfType(target));
        if (type != null)
        {
            foreach (var propertyName in propertyNames[type])
            {
                try
                {
                    PropertyInfo info = GetPropertyInfo(propertyName, target);
                    if (info.PropertyType.IsPrimitive)
                    {
                        Properties.Add(new PropertyNameValuePair(
                            propertyName, GetValueByPropertyName(propertyName, target).ToString()));
                    }
                    else
                    {
                        Properties.Add(new PropertyNameValuePair(
                            propertyName, JsonUtility.ToJson(GetValueByPropertyName(propertyName, target))));
                    }
                }
                catch (Exception e)
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
    public void SetPropertyValueByComponent(object target)
    {
        foreach (var property in Properties)
        {
            PropertyInfo info = GetPropertyInfo(property.Key, target);
            Type PropertyType = info?.PropertyType;
            object obj = null;
            if (PropertyType.IsPrimitive)
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
