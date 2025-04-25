using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Responsible for highlighting a button in the UI when the player hovers over it
/// </summary>
public class ButtonHover : MonoBehaviour
{
    /// <summary>
    /// Triggers when the player hovers over the button
    /// </summary>
    public void OnHover()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
