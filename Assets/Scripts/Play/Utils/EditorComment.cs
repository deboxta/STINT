#if UNITY_EDITOR
using UnityEngine;

/// <summary>
/// Attach this script to any gameObject for which you want to put a note.
/// </summary>
public class EditorComment : MonoBehaviour
{
    [TextArea]
    public string comment = "";
}
#endif