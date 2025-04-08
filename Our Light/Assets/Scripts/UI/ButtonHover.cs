using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour
{
    public void OnHover()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
