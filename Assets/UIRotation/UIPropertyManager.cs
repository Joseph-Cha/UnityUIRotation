using UnityEngine;
using UnityEditor;

public class UIPropertyManager : MonoBehaviour
{    
    private static UIPropertyManager instance = null;
    public static UIPropertyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIPropertyManager>();
            }
            return instance;
        }
    }
    public delegate void Load();
    public event Load OnLoadEventHandler;
    private ScreenOrientationState ScreenOrientationState = new ScreenOrientationState();    
    private ScreenOrientation currentOrientationType;

    
    private void Start()
    {
#if UNITY_EDITOR
        currentOrientationType = ScreenOrientationState.CurrentOrientaion();
#endif
        currentOrientationType = Screen.orientation;
    }
    
    private void Update()
    {
#if UNITY_EDITOR
        if(currentOrientationType != ScreenOrientationState.CurrentOrientaion())
        {
            currentOrientationType = ScreenOrientationState.CurrentOrientaion();
            OnLoadEventHandler();
        }
#else
        if(currentOrientationType != Screen.orientation )
        {
            currentOrientationType = Screen.orientation;
            OnLoadEventHandler();
        }
#endif
    }

#if UNITY_EDITOR

    [MenuItem("ComponentProperty/Load #l")] 
    static public void OnLoadMenu()
    {
        if(!Selection.activeGameObject)
            return;        
        var UIPropertyManager = Selection.activeGameObject.GetComponent<UIPropertyManager>();
        UIPropertyManager.OnLoadEventHandler();
    }
#endif
}
