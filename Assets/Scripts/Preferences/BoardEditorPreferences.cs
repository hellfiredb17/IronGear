using UnityEngine;


[CreateAssetMenu(menuName = "IronGears/BoardEditorPreference")]
public class BoardEditorPreferences : ScriptableObject
{
    //---- Input Controls
    //-------------------
    [Header("Input Controls")]

    public Vector2 ZoomClamp = new Vector2(1, 30);
    public float DragThreshold = 1.0f;
    public float ZoomInterval = 5.0f;
    public float CameraSpeed = 10.0f;
}
