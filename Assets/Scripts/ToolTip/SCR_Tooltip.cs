using System.Collections;
using UnityEngine;
using TMPro;

public class SCR_Tooltip : MonoBehaviour {
    public TextMeshProUGUI tooltipText;
    public RectTransform tooltipBackground;
    public Vector2 offset = new Vector2(1f, -1f);
    //private RectTransform rectTransform;

    private void Start() {
        //rectTransform = GetComponent<RectTransform>();
        tooltipBackground.gameObject.SetActive(false);
        tooltipText.gameObject.SetActive(false);

    }

    private void Update() {
        //transform.position = Input.mousePosition + (Vector3)offset;
    }

    public void ShowTooltip(string text, Transform buttonTransform) {
        tooltipText.text = text;
        tooltipBackground.gameObject.SetActive(true);
        tooltipText.gameObject.SetActive(true);
        Vector3 newPosition = buttonTransform.position + (Vector3)offset;
        tooltipBackground.position = newPosition;
    }

    public void HideTooltip() {
        tooltipBackground.gameObject.SetActive(false);
        tooltipText.gameObject.SetActive(false);
    }
}