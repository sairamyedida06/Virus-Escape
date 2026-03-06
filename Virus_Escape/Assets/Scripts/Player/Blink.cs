using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] GameObject[] Target;

    [SerializeField] float OnDuration = 0.2f;

    [SerializeField] float offDuration = 0.4f;

    float timer = 0f;

    bool state = true;

    float blinkStopTime = 0f;
    public void ActiveBlink(float duration)
    {
        blinkStopTime = Time.time + duration;
    }

    private void Update()
    {
        if (Time.time <= blinkStopTime)
        {
            UpdateBlink();
        }
        else
        {
            SetTargetActive(true);
        }

    }

    public void UpdateBlink()
    {
        SetTargetActive(true);

        if (state == true)
        {
            if (timer >= OnDuration)
            {
                timer = 0f;
                state = false;
            }
        }
           
        else
        {
            SetTargetActive(false);

            if (timer >= offDuration)
            {
                timer = 0f;
                state = true;
            }
        }

        timer += Time.deltaTime;
    }

    public void SetTargetActive(bool active)
    {
        for(int i = 0; i < Target.Length; i++) 
        { 
            Target[i].SetActive(active);
        }

    }


}
