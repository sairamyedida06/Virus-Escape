using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private float points;
    [SerializeField] private float maxPoints;


    [SerializeField] private float staminaRegeneration;
    [SerializeField] private float regenDelay;
    private float allowRegenTIme = 0f;
    public float Points
    {
        get
        {
            return points;
        }

        set 
        {
            points = Mathf.Clamp(value, 0f, maxPoints);  
        }
    }

    public float MaxPoints => maxPoints;

    public float FillAmount => Points/MaxPoints;


    private void Update()
    {
        if(Time.time >= allowRegenTIme)
        {
            Points += staminaRegeneration * Time.deltaTime;
        }

        UI_Manager.Instance.SetStaminaFillAmount(FillAmount);
    }
    public bool HasEnoughStamina(float stamina)
    {
        return stamina < points;
    }

    public void ConsumeStamina(float stamina)
    {
        if(Points < 0.001f)
        {
            return;
        }

        points -= stamina;  

        allowRegenTIme = Time.time + regenDelay;
    }
}
