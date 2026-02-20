using UnityEngine;
using UnityEngine.Rendering;

public class Blink : MonoBehaviour
{
    [SerializeField] GameObject [] Target;

    public bool State;

    [SerializeField] float onDuration;
    [SerializeField] float offDuration;

    float stopDuration;


    [SerializeField] float timer;



    private void Update()
    {
        if(timer <= stopDuration)
        {
            UpdateBlink();
        }
        else
        {
            SetTargetActive(true);
        }
    }

    void ActiveBlink(float duration)
    {
        stopDuration = Time.time + duration;
    }
    void UpdateBlink()
    {
        if( State == true)
        {
            SetTargetActive(true);
        }
        else
        {
            SetTargetActive(false);
        }

        timer += Time.deltaTime;
    }

    void SetTargetActive(bool active)
    {
        for(int i = 0; i < Target.Length; i++)
        {
            Target[i].SetActive(active);
        }
    }
}
