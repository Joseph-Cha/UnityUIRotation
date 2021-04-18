using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;

public class UIRotation_PlayModeTest
{
    private Vector2 vector2 = Vector2.up;
    private Vector3 vector3 = Vector3.forward;
    private Quaternion quaternion = Quaternion.Euler(Vector3.forward);
    private GridLayoutGroup.Corner corner = GridLayoutGroup.Corner.LowerLeft;
    private GridLayoutGroup.Axis axis = GridLayoutGroup.Axis.Vertical;
    private GridLayoutGroup.Constraint constraint = GridLayoutGroup.Constraint.Flexible;
    private RectOffset rectOffset = new RectOffset(2, 2, 2, 2);
    private TextAnchor textAnchor = TextAnchor.MiddleCenter;
    private TextAlignmentOptions textAlignmentOptions = TextAlignmentOptions.TopRight;
    private HorizontalAlignmentOptions horizontalAlignmentOptions = HorizontalAlignmentOptions.Right;
    private VerticalAlignmentOptions verticalAlignmentOptions = VerticalAlignmentOptions.Middle;
    private FontStyles fontStyles = FontStyles.Italic;
    private ScrollRect.MovementType movementType = UnityEngine.UI.ScrollRect.MovementType.Elastic;
    private ScrollRect.ScrollbarVisibility scrollbarVisibility = UnityEngine.UI.ScrollRect.ScrollbarVisibility.Permanent;
    private string name = "Virnect";
    private float value = 5f;
    private int count = 1;
    private bool isActive = true;

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest, Order(0)]
    public IEnumerator SaveTestWithEnumeratorPasses()
    {
        ScreenOrientationState ScreenOrientationState = new ScreenOrientationState();
        string currentOrientation =  ScreenOrientationState.GetPathByOrientation();
        GameObject manager = new GameObject();
        manager.AddComponent<ComponentsManager>();
        // Arrange
        // UI Arrange 
        GameObject parent = new GameObject("TestRunner");
        GameObject child1 = new GameObject("child1");
        GameObject child2 = new GameObject("child2");
        GameObject gchild1 = new GameObject("gchild1");
        GameObject gchild2 = new GameObject("gchild2");
        GameObject gchild3 = new GameObject("gchild3");
        GameObject gchild4 = new GameObject("gchild4");

        parent.AddComponent<Canvas>();
        
        RectTransform rectTransform_c2 = child2.AddComponent<RectTransform>();
        RectTransform rectTransform_c1 = child1.AddComponent<RectTransform>();
        RectTransform rectTransform_gc1 =  gchild1.AddComponent<RectTransform>();
        RectTransform rectTransform_gc2 =  gchild2.AddComponent<RectTransform>();
        RectTransform rectTransform_gc3 =  gchild3.AddComponent<RectTransform>();
        RectTransform rectTransform_gc4 =  gchild4.AddComponent<RectTransform>();
        VerticalLayoutGroup verticalLayout_c1 = child1.AddComponent<VerticalLayoutGroup>();
        HorizontalLayoutGroup horizontalLayout_c2 = child2.AddComponent<HorizontalLayoutGroup>();
        ScrollRect scrollRect_gc1 = gchild1.AddComponent<ScrollRect>();
        TextMeshProUGUI textMeshPro_gc2 = gchild2.AddComponent<TextMeshProUGUI>();
        LayoutElement layout_gc3 = gchild3.AddComponent<LayoutElement>();
        GridLayoutGroup gridLayout_gc4 = gchild4.AddComponent<GridLayoutGroup>();

        child1.transform.SetParent(parent.transform);
        child2.transform.SetParent(parent.transform);
        gchild1.transform.SetParent(child1.transform);
        gchild2.transform.SetParent(child1.transform);
        gchild3.transform.SetParent(child2.transform);
        gchild4.transform.SetParent(child2.transform);
        
        RectTransformSetting(rectTransform_c1);
        RectTransformSetting(rectTransform_c2);
        VerticalLayoutGroupSetting(verticalLayout_c1);
        HorizontalLayoutGroupSetting(horizontalLayout_c2);
        ScrollRectSetting(scrollRect_gc1);
        TMP_TextSetting(textMeshPro_gc2);
        LayoutElementSetting(layout_gc3);
        GridLayoutGroupSetting(gridLayout_gc4);



        ComponentProperty ComponentProperty = parent.AddComponent<ComponentProperty>();
        ComponentProperty.Root = parent.transform;
        Selection.activeGameObject = parent;
        
        string resourcePath = $"{currentOrientation}/{SceneManager.GetActiveScene().name}/{parent.name}";

        // 세팅을 하기 위해 한 프레임 건너뜀
        yield return null;
        
        // Act
        ComponentProperty.OnSaveMenu();
        TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);
        ComponentsNode node = JsonUtility.FromJson<ComponentsNode>(jsonFile.text);
        // Assert
        Assert.IsNotNull(jsonFile);
    }

    [UnityTest, Order(1)]
    public IEnumerator LoadTestWithEnumeratorPasses()
    {
        GameObject manager = new GameObject();
        manager.AddComponent<ComponentsManager>();
        // Arrange
        GameObject parent = new GameObject("TestRunner");
        GameObject child1 = new GameObject("child1");
        GameObject child2 = new GameObject("child2");
        GameObject gchild1 = new GameObject("gchild1");
        GameObject gchild2 = new GameObject("gchild2");
        GameObject gchild3 = new GameObject("gchild3");
        GameObject gchild4 = new GameObject("gchild4");

        parent.AddComponent<Canvas>();
        RectTransform rectC1 = child1.AddComponent<RectTransform>();
        RectTransform rectC2 = child2.AddComponent<RectTransform>();
        RectTransform rectG1 = gchild1.AddComponent<RectTransform>();
        RectTransform rectG2 = gchild2.AddComponent<RectTransform>();
        RectTransform rectG3 = gchild3.AddComponent<RectTransform>();
        RectTransform rectG4 = gchild4.AddComponent<RectTransform>();
        VerticalLayoutGroup verticalC1 = child1.AddComponent<VerticalLayoutGroup>();
        HorizontalLayoutGroup horizontalC1 = child2.AddComponent<HorizontalLayoutGroup>();
        ScrollRect scrollRectG1 = gchild1.AddComponent<ScrollRect>();
        TextMeshProUGUI tmpG2 = gchild2.AddComponent<TextMeshProUGUI>();
        LayoutElement layoutElementG3 = gchild3.AddComponent<LayoutElement>();
        GridLayoutGroup gridLayoutG4 = gchild4.AddComponent<GridLayoutGroup>();
        ComponentProperty ComponentProperty = parent.AddComponent<ComponentProperty>();
        
        child1.transform.SetParent(parent.transform);
        child2.transform.SetParent(parent.transform);
        gchild1.transform.SetParent(child1.transform);
        gchild2.transform.SetParent(child1.transform);
        gchild3.transform.SetParent(child2.transform);
        gchild4.transform.SetParent(child2.transform);
        ComponentProperty.Root = parent.transform;

        bool result = false;
        
        yield return null;

        // Act
        ComponentProperty.Load();

        if (AssertRectTransform(rectC1) 
            && AssertRectTransform(rectC2) 
            && AssertVerticalLayoutGroup(verticalC1) 
            && AssertHorizontalLayoutGroup(horizontalC1)
            && AssertScrollRect(scrollRectG1) 
            && AssertTextMeshProUGUI(tmpG2) 
            && AssertLayoutElement(layoutElementG3)
            && AssertGridLayoutGroup(gridLayoutG4))
            result = true;
        
        // Assert
        Assert.IsTrue(result);
    }
    #region Setting
    private void RectTransformSetting(RectTransform rect)
    {
        rect.sizeDelta = vector2;
        rect.pivot = vector2;
        rect.anchorMin = vector2;
        rect.anchorMax = vector2;
        // rect.localScale = vector3;
        // rect.localPosition = vector3;
        // rect.localRotation = quaternion;
        rect.anchoredPosition = vector2;
    }
    private void LayoutElementSetting(LayoutElement layout)
    {
        layout.minWidth = value;
        layout.minHeight = value;
        layout.preferredWidth = value;
        layout.preferredHeight = value;
        layout.flexibleWidth = value;
        layout.flexibleHeight = value;
    }
    private void GridLayoutGroupSetting(GridLayoutGroup gridLayout)
    {
        gridLayout.startCorner = corner;
        gridLayout.startAxis = axis;
        gridLayout.cellSize = vector2;
        gridLayout.spacing = vector2;
        gridLayout.constraint = constraint;
        gridLayout.constraintCount = count;
        gridLayout.padding = rectOffset;
        gridLayout.childAlignment = textAnchor;
    }
    private void VerticalLayoutGroupSetting(VerticalLayoutGroup verticalLayout)
    {
        verticalLayout.spacing = value;
        verticalLayout.childForceExpandWidth = isActive;
        verticalLayout.childForceExpandHeight = isActive;
        verticalLayout.childControlWidth = isActive;
        verticalLayout.childControlHeight = isActive;
        verticalLayout.childScaleWidth = isActive;
        verticalLayout.childScaleHeight = isActive;
        verticalLayout.reverseArrangement = isActive;
        verticalLayout.padding = rectOffset;
        verticalLayout.childAlignment = textAnchor;     
    }
    private void HorizontalLayoutGroupSetting(HorizontalLayoutGroup horizontalLayout)
    {
        horizontalLayout.spacing = value;
        horizontalLayout.childForceExpandWidth = isActive;
        horizontalLayout.childForceExpandHeight = isActive;
        horizontalLayout.childControlWidth = isActive;
        horizontalLayout.childControlHeight = isActive;
        horizontalLayout.childScaleWidth = isActive;
        horizontalLayout.childScaleHeight = isActive;
        horizontalLayout.reverseArrangement = isActive;
        horizontalLayout.padding = rectOffset;
        horizontalLayout.childAlignment = textAnchor;    
    }
    private void TMP_TextSetting(TextMeshProUGUI tmp)
    {
        tmp.alignment = textAlignmentOptions;
        tmp.horizontalAlignment = horizontalAlignmentOptions;
        tmp.verticalAlignment = verticalAlignmentOptions;
        tmp.autoSizeTextContainer = isActive;
        tmp.fontSize = value;
        tmp.fontSizeMin = value;
        tmp.fontSizeMax = value;
        tmp.characterSpacing = value;
        tmp.wordSpacing = value;
        tmp.lineSpacing = value;
        tmp.lineSpacingAdjustment = value;
        tmp.paragraphSpacing = value;
        tmp.textStyle.name = name; 
        tmp.fontStyle = fontStyles;
    }
    private void ScrollRectSetting(ScrollRect scrollRect)
    {
        scrollRect.horizontal = isActive;
        scrollRect.vertical = isActive;
        scrollRect.movementType = movementType;
        scrollRect.elasticity = value;
        scrollRect.inertia = isActive;
        scrollRect.decelerationRate = value;
        scrollRect.scrollSensitivity = value;
        scrollRect.horizontalScrollbarVisibility = scrollbarVisibility;
        scrollRect.verticalScrollbarVisibility = scrollbarVisibility;
        scrollRect.horizontalScrollbarSpacing = value;
        scrollRect.verticalScrollbarSpacing = value;
    }
    #endregion

    #region Assert
    private bool AssertRectTransform(RectTransform rect)
    {
        bool result = false;
        if(rect.sizeDelta == vector2
            && rect.pivot == vector2
            && rect.anchorMin == vector2 
            && rect.anchorMax == vector2
            // && rect.localScale == vector3
            // && rect.localPosition == vector3
            // && rect.localRotation == quaternion
            && rect.anchoredPosition == vector2)
            result = true;
        return result;
    }
    private bool AssertLayoutElement(LayoutElement layout)
    {
        bool result = false;
        if(layout.minWidth == value
            && layout.minHeight == value 
            && layout.preferredWidth == value 
            && layout.preferredHeight == value 
            && layout.flexibleWidth == value 
            && layout.flexibleHeight == value)
            result = true;
        return result;
    }
    private bool AssertGridLayoutGroup(GridLayoutGroup gridLayout)
    {
        bool result = false;
        if(gridLayout.startCorner == corner
            && gridLayout.startAxis == axis
            && gridLayout.cellSize == vector2
            && gridLayout.spacing == vector2
            && gridLayout.constraint == constraint
            && gridLayout.constraintCount == count
            && gridLayout.padding == rectOffset
            && gridLayout.childAlignment == textAnchor)
            result = true;
        return result;
    }
    
    private bool AssertVerticalLayoutGroup(VerticalLayoutGroup verticalLayout)
    {
        bool result = false;
        if(verticalLayout.spacing == value
            && verticalLayout.childForceExpandWidth == isActive
            && verticalLayout.childForceExpandHeight == isActive
            && verticalLayout.childControlWidth == isActive
            && verticalLayout.childControlHeight == isActive
            && verticalLayout.childScaleWidth == isActive
            && verticalLayout.childScaleHeight == isActive
            && verticalLayout.reverseArrangement == isActive
            && verticalLayout.padding == rectOffset
            && verticalLayout.childAlignment == textAnchor)
            result = true;
        return result;
    }
    private bool AssertHorizontalLayoutGroup(HorizontalLayoutGroup horizontalLayout)
    {
        bool result = false;
        if(horizontalLayout.spacing == value
            && horizontalLayout.childForceExpandWidth == isActive
            && horizontalLayout.childForceExpandHeight == isActive
            && horizontalLayout.childControlWidth == isActive
            && horizontalLayout.childControlHeight == isActive
            && horizontalLayout.childScaleWidth == isActive
            && horizontalLayout.childScaleHeight == isActive
            && horizontalLayout.reverseArrangement == isActive
            && horizontalLayout.padding == rectOffset
            && horizontalLayout.childAlignment == textAnchor)
            result = true;
        return result;
    }
    private bool AssertTextMeshProUGUI(TextMeshProUGUI tmp)
    {
        bool result = false;
        if (tmp.alignment == textAlignmentOptions
            && tmp.horizontalAlignment == horizontalAlignmentOptions
            && tmp.verticalAlignment == verticalAlignmentOptions
            && tmp.autoSizeTextContainer == isActive
            && tmp.fontSize == value
            && tmp.fontSizeMin == value
            && tmp.fontSizeMax == value
            && tmp.characterSpacing == value
            && tmp.wordSpacing == value
            && tmp.lineSpacing == value
            && tmp.lineSpacingAdjustment == value
            && tmp.paragraphSpacing == value
            && tmp.textStyle.name == name 
            && tmp.fontStyle == fontStyles)
            result = true;
        return result;
    }
    private bool AssertScrollRect(ScrollRect scrollRect)
    {
        bool result = false;
        if(scrollRect.horizontal == isActive
            && scrollRect.vertical == isActive
            && scrollRect.movementType == movementType
            && scrollRect.elasticity == value
            && scrollRect.inertia == isActive
            && scrollRect.decelerationRate == value
            && scrollRect.scrollSensitivity == value
            && scrollRect.horizontalScrollbarVisibility == scrollbarVisibility
            && scrollRect.verticalScrollbarVisibility == scrollbarVisibility
            && scrollRect.horizontalScrollbarSpacing == value
            && scrollRect.verticalScrollbarSpacing == value)
            result = true;
        return result;
    }
    #endregion
}
