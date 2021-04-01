using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections.Generic;

// 하나의 Root Transform에서 가지고 있는 components들의 속성 정보를 저장
[Serializable]
public class ComponentInfo
{
    public ComponentInfo(Component component)
    {
        // Type type = typeof(Component);
        // PropertyInfo[] Components = type.GetProperties();    
        JsonSerializerSettings JsonSetting = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        switch(component)
        {
            case RectTransform rectTransform:
                // PropertyInfo[] rectTransforminfo = rectTransform.GetType().GetProperties();
                // foreach(var info in rectTransforminfo)
                // {
                //     ComponentInfos.Add(info.Name, info.GetValue(component).ToString());
                // }
                ComponentInfos.Add(nameof(rectTransform.sizeDelta), JsonConvert.SerializeObject(rectTransform.sizeDelta, JsonSetting));
                ComponentInfos.Add(nameof(rectTransform.pivot), JsonConvert.SerializeObject(rectTransform.pivot, JsonSetting));
                ComponentInfos.Add(nameof(rectTransform.anchorMin), JsonConvert.SerializeObject(rectTransform.anchorMin, JsonSetting));
                ComponentInfos.Add(nameof(rectTransform.anchorMax), JsonConvert.SerializeObject(rectTransform.anchorMax, JsonSetting));
                ComponentInfos.Add(nameof(rectTransform.localScale), JsonConvert.SerializeObject(rectTransform.localScale, JsonSetting));
                ComponentInfos.Add(nameof(rectTransform.localPosition), JsonConvert.SerializeObject(rectTransform.localPosition, JsonSetting));
                ComponentInfos.Add(nameof(rectTransform.localRotation), JsonConvert.SerializeObject(rectTransform.localRotation, JsonSetting));
                ComponentInfos.Add(nameof(rectTransform.anchoredPosition), JsonConvert.SerializeObject(rectTransform.anchoredPosition, JsonSetting));
                // // RectTransformInfos = rectTransform.GetType().GetProperties();
                // RectTransformInfos = rectTransform.GetType().GetProperties();
                break;

            case LayoutElement layoutElement:
                ComponentInfos.Add(nameof(layoutElement.minWidth), JsonConvert.SerializeObject(layoutElement.minWidth, JsonSetting));
                ComponentInfos.Add(nameof(layoutElement.minHeight), JsonConvert.SerializeObject(layoutElement.minHeight, JsonSetting));
                ComponentInfos.Add(nameof(layoutElement.preferredWidth), JsonConvert.SerializeObject(layoutElement.preferredWidth, JsonSetting));
                ComponentInfos.Add(nameof(layoutElement.preferredHeight), JsonConvert.SerializeObject(layoutElement.preferredHeight, JsonSetting));
                ComponentInfos.Add(nameof(layoutElement.flexibleWidth), JsonConvert.SerializeObject(layoutElement.flexibleWidth, JsonSetting));
                ComponentInfos.Add(nameof(layoutElement.flexibleHeight), JsonConvert.SerializeObject(layoutElement.flexibleHeight, JsonSetting));
                // LayoutElementInfos = layoutElement.GetType().GetProperties();
                // LayoutElementInfos.ToList().ForEach(info => Debug.Log($"LayoutElement Info Name : {info.Name}"));
                break;

            case VerticalLayoutGroup verticalLayoutGroup:
                ComponentInfos.Add(nameof(verticalLayoutGroup.spacing), JsonConvert.SerializeObject(verticalLayoutGroup.spacing, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.childForceExpandWidth), JsonConvert.SerializeObject(verticalLayoutGroup.childForceExpandWidth, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.childForceExpandHeight), JsonConvert.SerializeObject(verticalLayoutGroup.childForceExpandHeight, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.childControlWidth), JsonConvert.SerializeObject(verticalLayoutGroup.childControlWidth, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.childControlHeight), JsonConvert.SerializeObject(verticalLayoutGroup.childControlHeight, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.childScaleWidth), JsonConvert.SerializeObject(verticalLayoutGroup.childScaleWidth, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.childScaleHeight), JsonConvert.SerializeObject(verticalLayoutGroup.childScaleHeight, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.reverseArrangement), JsonConvert.SerializeObject(verticalLayoutGroup.reverseArrangement, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.padding), JsonConvert.SerializeObject(verticalLayoutGroup.padding, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.childAlignment), JsonConvert.SerializeObject(verticalLayoutGroup.childAlignment, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.minWidth), JsonConvert.SerializeObject(verticalLayoutGroup.minWidth, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.preferredWidth), JsonConvert.SerializeObject(verticalLayoutGroup.preferredWidth, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.flexibleWidth), JsonConvert.SerializeObject(verticalLayoutGroup.flexibleWidth, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.minHeight), JsonConvert.SerializeObject(verticalLayoutGroup.minHeight, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.preferredHeight), JsonConvert.SerializeObject(verticalLayoutGroup.preferredHeight, JsonSetting));
                ComponentInfos.Add(nameof(verticalLayoutGroup.flexibleHeight), JsonConvert.SerializeObject(verticalLayoutGroup.flexibleHeight, JsonSetting));
                // VerticalLayoutGroupInfos = verticalLayoutGroup.GetType().GetProperties();
                // VerticalLayoutGroupInfos.ToList().ForEach(info => Debug.Log($"Vertical LayoutGroup Info Name : {info.Name}"));
                break;

            case GridLayoutGroup gridLayoutGroup:
                ComponentInfos.Add(nameof(gridLayoutGroup.startCorner), JsonConvert.SerializeObject(gridLayoutGroup.startCorner, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.startAxis), JsonConvert.SerializeObject(gridLayoutGroup.startAxis, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.cellSize), JsonConvert.SerializeObject(gridLayoutGroup.cellSize, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.spacing), JsonConvert.SerializeObject(gridLayoutGroup.spacing, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.constraint), JsonConvert.SerializeObject(gridLayoutGroup.constraint, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.constraintCount), JsonConvert.SerializeObject(gridLayoutGroup.constraintCount, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.padding), JsonConvert.SerializeObject(gridLayoutGroup.padding, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.childAlignment), JsonConvert.SerializeObject(gridLayoutGroup.childAlignment, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.minWidth), JsonConvert.SerializeObject(gridLayoutGroup.minWidth, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.preferredWidth), JsonConvert.SerializeObject(gridLayoutGroup.preferredWidth, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.flexibleWidth), JsonConvert.SerializeObject(gridLayoutGroup.flexibleWidth, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.minHeight), JsonConvert.SerializeObject(gridLayoutGroup.minHeight, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.preferredHeight), JsonConvert.SerializeObject(gridLayoutGroup.preferredHeight, JsonSetting));
                ComponentInfos.Add(nameof(gridLayoutGroup.flexibleHeight), JsonConvert.SerializeObject(gridLayoutGroup.flexibleHeight, JsonSetting));
                // GridLayoutGroupInfos = gridLayoutGroup.GetType().GetProperties();
                // GridLayoutGroupInfos.ToList().ForEach(info => Debug.Log($"GridLayoutGroupInfos Name : {info.Name}"));
                break;

            case TMP_Text text:
                ComponentInfos.Add(nameof(text.alignment), JsonConvert.SerializeObject(text.alignment, JsonSetting));
                ComponentInfos.Add(nameof(text.horizontalAlignment), JsonConvert.SerializeObject(text.horizontalAlignment, JsonSetting));
                ComponentInfos.Add(nameof(text.verticalAlignment), JsonConvert.SerializeObject(text.verticalAlignment, JsonSetting));
                ComponentInfos.Add(nameof(text.autoSizeTextContainer), JsonConvert.SerializeObject(text.autoSizeTextContainer, JsonSetting));
                ComponentInfos.Add(nameof(text.fontSize), JsonConvert.SerializeObject(text.fontSize, JsonSetting));
                ComponentInfos.Add(nameof(text.fontSizeMin), JsonConvert.SerializeObject(text.fontSizeMin, JsonSetting));
                ComponentInfos.Add(nameof(text.fontSizeMax), JsonConvert.SerializeObject(text.fontSizeMax, JsonSetting));
                ComponentInfos.Add(nameof(text.characterSpacing), JsonConvert.SerializeObject(text.characterSpacing, JsonSetting));
                ComponentInfos.Add(nameof(text.wordSpacing), JsonConvert.SerializeObject(text.wordSpacing, JsonSetting));
                ComponentInfos.Add(nameof(text.lineSpacing), JsonConvert.SerializeObject(text.lineSpacing, JsonSetting));
                ComponentInfos.Add(nameof(text.lineSpacingAdjustment), JsonConvert.SerializeObject(text.lineSpacingAdjustment, JsonSetting));
                ComponentInfos.Add(nameof(text.paragraphSpacing), JsonConvert.SerializeObject(text.paragraphSpacing, JsonSetting));
                ComponentInfos.Add(nameof(text.textStyle), JsonConvert.SerializeObject(text.textStyle, JsonSetting));
                ComponentInfos.Add(nameof(text.fontStyle), JsonConvert.SerializeObject(text.fontStyle, JsonSetting));
                // TMPTextInfos = text.GetType().GetProperties();
                // TMPTextInfos.ToList().ForEach(info => Debug.Log($"TMPTextInfos Name : {info.Name}"));
                break;

            case ScrollRect scrollRect:
                ComponentInfos.Add(nameof(scrollRect.horizontal), JsonConvert.SerializeObject(scrollRect.horizontal, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.vertical), JsonConvert.SerializeObject(scrollRect.vertical, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.movementType), JsonConvert.SerializeObject(scrollRect.movementType, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.elasticity), JsonConvert.SerializeObject(scrollRect.elasticity, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.inertia), JsonConvert.SerializeObject(scrollRect.inertia, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.decelerationRate), JsonConvert.SerializeObject(scrollRect.decelerationRate, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.scrollSensitivity), JsonConvert.SerializeObject(scrollRect.scrollSensitivity, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.horizontalScrollbar), JsonConvert.SerializeObject(scrollRect.horizontalScrollbar, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.verticalScrollbar), JsonConvert.SerializeObject(scrollRect.verticalScrollbar, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.horizontalScrollbarVisibility), JsonConvert.SerializeObject(scrollRect.horizontalScrollbarVisibility, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.verticalScrollbarVisibility), JsonConvert.SerializeObject(scrollRect.verticalScrollbarVisibility, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.horizontalScrollbarSpacing), JsonConvert.SerializeObject(scrollRect.horizontalScrollbarSpacing, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.verticalScrollbarSpacing), JsonConvert.SerializeObject(scrollRect.verticalScrollbarSpacing, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.onValueChanged), JsonConvert.SerializeObject(scrollRect.onValueChanged, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.velocity), JsonConvert.SerializeObject(scrollRect.velocity, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.normalizedPosition), JsonConvert.SerializeObject(scrollRect.normalizedPosition, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.horizontalNormalizedPosition), JsonConvert.SerializeObject(scrollRect.horizontalNormalizedPosition, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.verticalNormalizedPosition), JsonConvert.SerializeObject(scrollRect.verticalNormalizedPosition, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.minWidth), JsonConvert.SerializeObject(scrollRect.minWidth, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.preferredWidth), JsonConvert.SerializeObject(scrollRect.preferredWidth, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.flexibleWidth), JsonConvert.SerializeObject(scrollRect.flexibleWidth, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.minHeight), JsonConvert.SerializeObject(scrollRect.minHeight, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.preferredHeight), JsonConvert.SerializeObject(scrollRect.preferredHeight, JsonSetting));
                ComponentInfos.Add(nameof(scrollRect.flexibleHeight), JsonConvert.SerializeObject(scrollRect.flexibleHeight, JsonSetting));
                // ScrollRectInfos = scrollRect.GetType().GetProperties();
                // ScrollRectInfos.ToList().ForEach(info => Debug.Log($"ScrollRectInfos Name : {info.Name}"));
                break;

            default:
                break;
        }
    }
    public Dictionary<string, string> ComponentInfos = new Dictionary<string, string>();

    // Root 트랜스폼을 가지고 와야함
    // GetPath로 현재 Transform 정보가 필요함
    // Transform transform = root.Find(Path)을 통해 현재 자신의 정보를 알아야하고
    // 거기서 GetComponent를 해서 해당 component를 찾아서 대입시켜야함.
    public void Apply(Component component)
    {
        switch(component)
        {
            case RectTransform rectTransform:
                break;
            case LayoutElement layoutElement:
                break;
            case VerticalLayoutGroup verticalLayoutGroup:
                break;
            case GridLayoutGroup gridLayoutGroup:
                break;
            case TMP_Text text:
                break;
            case ScrollRect scrollRect:
                break;
        }
    }

    // 향후 componet의 Property 값을 추가 하기 위해 삭제 금지
    // private PropertyInfo[] RectTransformInfos;
    // private PropertyInfo[] LayoutElementInfos;
    // private PropertyInfo[] VerticalLayoutGroupInfos;
    // private PropertyInfo[] GridLayoutGroupInfos;
    // private PropertyInfo[] TMPTextInfos;
    // private PropertyInfo[] TextInfos;
    // private PropertyInfo[] ScrollRectInfos;
}
