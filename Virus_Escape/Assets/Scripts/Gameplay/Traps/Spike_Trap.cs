using System;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType
{
    pressure,
    looping
}

public class Spike_Trap : MonoBehaviour
{
    public TrapType trap = TrapType.looping;


    [SerializeField] bool activeOnStart = false;
    [SerializeField] float activationDelay;
    [SerializeField] float activeDuration;
    [SerializeField] float transitionDuration;

    [SerializeField] Transform spikeMesh;

    [SerializeField] Vector3 spikeMeshIdlePosition = new Vector3(0f, -0.508f, 0f);
    [SerializeField] Vector3 spikeMeshActivePosition = new Vector3(0, .12f, 0);

    [SerializeField] AudioSource localSource;
    [SerializeField] private AudioClip spikeClip;


    float timer;
    

    public List<IDamageable> damageables = new List<IDamageable>();

    public enum TransitionState
    {
        Idle,
        wait,
        TransistionToActive,
        Active,
        TransitionToIdle
    }

    TransitionState state = TransitionState.Idle;

    private void Start()
    {
        if(trap == TrapType.pressure)
    {
            
            ChangeState(activeOnStart ? TransitionState.wait : TransitionState.Idle);
        }
    else 
        {
            ChangeState(activeOnStart ? TransitionState.TransistionToActive : TransitionState.wait);
        }

    }
    public void ChangeState(TransitionState newState)
    {
        state = newState;

        timer = 0f;

        if (state == TransitionState.Active) 
        {
            ApplyDamage();
            
        }

        if(state == TransitionState.TransistionToActive)
        {
            localSource.PlayOneShot(spikeClip);
            VFX_Manager.Instance.PlayLandVFX(transform.position);
        }
    }

    void Update()
    {
        UpdateFSM();
    }

    void UpdateFSM()
    {
        switch (state)
        {
            case TransitionState.wait:
                spikeMesh.localPosition = spikeMeshIdlePosition;

                if(timer >= activationDelay)
                {
                    ChangeState(TransitionState.TransistionToActive);
                }

                break;

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
                    ChangeState(trap == TrapType.looping? TransitionState.TransistionToActive:TransitionState.Idle);
                    
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


    private void OnTriggerEnter(Collider other)
    {
        IDamageable health = other.GetComponentInParent<IDamageable>();

        if (health != null && !damageables.Contains(health))
        {
            damageables.Add(health);
        }

        if (health != null && state == TransitionState.Idle && trap == TrapType.pressure)
        {
            ChangeState(TransitionState.wait);
        }   

        if (health != null && state == TransitionState.Active)
        {
            health.Damage(1);
        }
            
    }
    void OnTriggerExit(Collider other)
    {
        IDamageable health = other.GetComponentInParent<IDamageable>();

        if (health != null && damageables.Contains(health))
        {
            damageables.Remove(health);
        }
            
    }

    public void ApplyDamage()
    {
       foreach(IDamageable health in damageables)
        {
            if(health != null)
            {
                health.Damage(1);
            }
        }
    }
}
