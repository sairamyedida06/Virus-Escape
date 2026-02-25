using UnityEngine;

public class Spike_Trap : MonoBehaviour
{
    [SerializeField] float activeDuration;
    [SerializeField] float transitionDuration;

    [SerializeField] Transform spikeMesh;

    [SerializeField] Vector3 spikeMeshIdlePosition = new Vector3(0f, -0.508f, 0f);
    [SerializeField] Vector3 spikeMeshActivePosition = new Vector3(0, .12f, 0);


    float timer;

    public enum TrapType
    {
        pressure,
        looping
    }

    TrapType trap = TrapType.pressure;

    public enum TransitionState
    {
        Idle,
        TransistionToActive,
        Active,
        TransitionToIdle
    }

    TransitionState state = TransitionState.Idle;

    private void Start()
    {
        
    }
    public void ChangeState(TransitionState newState)
    {
        state = newState;

        timer = 0f;
    }

    void Update()
    {
        UpdateFSM();
    }

    void UpdateFSM()
    {
        switch (state)
        {
            case TransitionState.TransistionToActive:

                spikeMesh.localPosition = Vector3.Lerp(spikeMeshIdlePosition, spikeMeshActivePosition, timer / transitionDuration);

                if (timer >= transitionDuration)
                {
                    ChangeState(TransitionState.Active);
                    
                }
                break;

            case TransitionState.TransitionToIdle:

                spikeMesh.localPosition = Vector3.Lerp(spikeMeshActivePosition, spikeMeshIdlePosition, timer / transitionDuration);

                if (timer >= transitionDuration)
                {
                    ChangeState(TransitionState.Idle);
                    
                }

                break;

            case TransitionState.Active:

                if (timer >= activeDuration)
                {
                    ChangeState(TransitionState.TransitionToIdle);
                    
                }

                break;
        }


        timer += Time.deltaTime; 
    }

    public void Activate()
    {
        if( state == TransitionState.Idle)
        {
            ChangeState(TransitionState.TransistionToActive);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Activate();
        
    }
}
