using UnityEngine;
using UnityEngine.EventSystems;

public class PointerAreaController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsPointerOver { get; private set; } = false;

    [SerializeField] private ImagePulseUI imagePulseUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsPointerOver = true;
        if (imagePulseUI != null)
        {
            imagePulseUI.PlayPulseAnimation();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsPointerOver = false;
        if (imagePulseUI != null)
        {
            imagePulseUI.StopAnimation();
        }
    }
}