using UnityEngine;

public class ComponentsManager : MonoBehaviour
{
    private static ComponentsManager instance;
    public static ComponentsManager Instance => instance ??= FindObjectOfType<ComponentsManager>();
    public delegate void ScreenRotationEventHandler();
    public event ScreenRotationEventHandler ScreenRotated;
    private ScreenOrientationState ScreenOrientationState = new ScreenOrientationState();
    private ScreenOrientation currentOrientationType;
    private void Awake()
    {
        if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    private void Start() => currentOrientationType = ScreenOrientationState.CurrentOrientaion();
    private void Update()
    {
        if (currentOrientationType != ScreenOrientationState.CurrentOrientaion())
        {
            currentOrientationType = ScreenOrientationState.CurrentOrientaion();
            ScreenRotated?.Invoke();
        }
    }
}
