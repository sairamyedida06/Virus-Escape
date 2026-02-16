using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class Player_Health : MonoBehaviour,IDamageable
{
    [SerializeField] int maxHealthPoints;

    public int healthPoints
    {
        get
        {
            return maxHealthPoints;
        }
        set 
        {
            bool wasAlive = maxHealthPoints > 0;
            maxHealthPoints = value;

            if(healthPoints < 0 && wasAlive)
            {
                Died.Invoke();
            }
        }
    }

    public bool alive => healthPoints > 0;


    private void Update()
    {
        if (alive)
        {
            if (transform.position.y < -1f)
            {
                OnFall();

            }
        }
    }

    private void OnFall()
    {
        Damage(healthPoints);
    }

    UnityEvent Damaged;
    UnityEvent Died;

    public void Damage(int damage)
    {
        healthPoints -= damage;

        if (alive)
        {
            Damaged.Invoke();
        }
       
    }
}
