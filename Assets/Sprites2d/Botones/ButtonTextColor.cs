using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ButtonTextColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color pressedColor = Color.green;

    private TMP_Text buttonText;

    private void Awake()
    {
        // Busca automáticamente el texto en los hijos
        buttonText = GetComponentInChildren<TMP_Text>();

        if (buttonText == null)
            Debug.LogWarning($"No se encontró TMP_Text en hijos de {gameObject.name}");
    }

    private void Start()
    {
        if (buttonText != null)
            buttonText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = pressedColor;
        SoundManager.Instance?.PlayClick();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = normalColor;
    }
}
