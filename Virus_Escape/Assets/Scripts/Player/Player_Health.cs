using UnityEngine;
using UnityEngine.Events;

public class Player_Health : MonoBehaviour,IDamageable
{
    [SerializeField] int maxHealthPoints;



    private void Update()
    {
        Fall();
    }


    public int HealthPoints
    {
        get
        {
            return maxHealthPoints;
        }
        set
        {
            bool wasAlive = HealthPoints > 0;
            maxHealthPoints = Mathf.Max(value, 0);
            maxHealthPoints = value;

            if (wasAlive && HealthPoints <= 0)
            {
                Died.Invoke();
            }
        }
    } 
        public UnityEvent Died;
        public UnityEvent Damaged;

    public bool Alive => HealthPoints > 0;

    void Fall()
    {
        if (Alive) 
        {
            if (transform.position.y <= -2f)
            {
                Damage(HealthPoints);

                
            }
        }
        
    }
    public void Damage(int damage)
    {
        if (!Alive) { return; }
        HealthPoints -= damage;

        Damaged.Invoke();



    }
}
