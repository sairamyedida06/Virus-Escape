using UnityEngine;

public class Trap_Spikes : MonoBehaviour
{
    [SerializeField] GameObject spikeMesh;
    [SerializeField] float spikeActiveDuration;
    [SerializeField] float spikeTransationDuration;

    [SerializeField] Vector3 spikesActivePosition = Vector3.zero;
    [SerializeField] Vector3 spikesIdlePosition = new Vector3(0, 0.51f, 0);



    float timer;



    enum Estate
    {
        Idle,
        wait,
        TransationToActive,
        Active,
        TransationToIdle
    }

    Estate state = Estate.Idle;

    void ChangeState(Estate newstate)
    {
        state = newstate;
        timer = 0f;
    }

    void UpdateFSM()
    {
        if (state == Estate.TransationToActive)
        {
            Vector3 p = Vector3.Lerp(spikesIdlePosition, spikesActivePosition, timer / spikeTransationDuration);
            spikeMesh.transform.localPosition = p;
           

            if(timer > spikeTransationDuration)
            {
                ChangeState(Estate.Active);
                
            }

        }
        else if (state == Estate.TransationToIdle)
        {
            Vector3 p = Vector3.Lerp(spikesActivePosition, spikesIdlePosition, timer / spikeTransationDuration);
            spikeMesh.transform.localPosition = p;
           

            if (timer > spikeTransationDuration)
            {
                ChangeState(Estate.Idle);
            }
        }
        else if (state == Estate.Active)
        {
            if (timer >= spikeActiveDuration)
            {
                ChangeState(Estate.TransationToIdle);

            }
        }



        timer += Time.deltaTime;
    }
    [ContextMenu("Activate Spike Trap")]
    public void Activate()
    {
        if (state == Estate.Idle)
        {
            ChangeState (Estate.TransationToActive);
        }
    }

    private void Update()
    {
        UpdateFSM();
    }



}
