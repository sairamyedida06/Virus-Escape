using UnityEngine;
using UnityEngine.UI;

public class Staminabar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] CanvasGroup CanvasGroup;

    private float staminaValue = 1f;


    public void SetValue(float value)
    {
        staminaValue = value;

        slider.value = value;

        
    }

    private void Start()
    {
        CanvasGroup.alpha = 0f;
    }
    private void Update()
    {
        float targetAlpha = staminaValue > 0.999f ? 0f : 1f;

        CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, targetAlpha, Time.deltaTime * 5f);
    }
}
