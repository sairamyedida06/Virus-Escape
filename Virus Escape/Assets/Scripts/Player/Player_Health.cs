using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class Player_Health : MonoBehaviour,IDamageable
{
    [SerializeField] int maxHealthPoints;

    public int HealthPoints
    {
        get
        {
            return maxHealthPoints;
        }
        set 
        {
            bool wasAlive = maxHealthPoints > 0;
            maxHealthPoints = value;

            if(HealthPoints <= 0 && wasAlive)
            {
                Died.Invoke();
                Debug.Log("player died");
                Uimanager.Instance.ShowGameOverMenu();
                
            }
        }
    }


    public UnityEvent Damaged;
    public UnityEvent Died;

    private void Update()
    {
        if (Alive)
        {
            if (transform.position.y < -1f)
            {
                OnFall();  
                
            }
        }
    }

    private void OnFall()
    {
        Damage(HealthPoints);
        Debug.Log("FELL");
        
         
    }

    

    public bool Alive => HealthPoints > 0;
    public void Damage(int damage)
    {
        HealthPoints -= damage;

        if (Alive)
        {
            Damaged.Invoke();
        }
       
    }
}
