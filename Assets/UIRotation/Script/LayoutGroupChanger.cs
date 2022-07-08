using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LayoutGroupChanger : MonoBehaviour
{
    public bool isReverse = false;
    private ScreenOrientationState ScreenOrientationState = new ScreenOrientationState();
    private void Awake() => ComponentsManager.Instance.ScreenRotated += Change;
    private void OnEnable() => Change();
    private void OnDestroy() => ComponentsManager.Instance.ScreenRotated -= Change;
#if UNITY_EDITOR
    [MenuItem("ComponentProperty/Change #c")]
    static public void OnSaveMenu()
    {
        if (!Selection.activeGameObject)
            return;
        var activeGameObject = Selection.activeGameObject;
        var LayoutGroupChangers = activeGameObject.GetComponentsInChildren<LayoutGroupChanger>();
        foreach (var LayoutGroupChanger in LayoutGroupChangers)
        {
            LayoutGroupChanger.Change();
        }
    }
#endif
    public void Change()
    {
        ScreenOrientation type = ScreenOrientationState.CurrentOrientaion();
        DestroyImmediate(gameObject.GetComponent<LayoutGroup>());

        switch (type)
        {
            case ScreenOrientation.Portrait:
                if (!isReverse)
                    gameObject.AddComponent<VerticalLayoutGroup>();
                else
                    gameObject.AddComponent<HorizontalLayoutGroup>();
                break;
            case ScreenOrientation.LandscapeRight:
            case ScreenOrientation.LandscapeLeft:
                if (!isReverse)
                    gameObject.AddComponent<HorizontalLayoutGroup>();
                else
                    gameObject.AddComponent<VerticalLayoutGroup>();
                break;
        }

        string currentName = transform.name;
        string layoutGroupName = gameObject.GetComponent<LayoutGroup>().GetType().Name;
        Debug.Log($"LayoutGroup of {currentName} has been changed in {layoutGroupName} ");
    }
}