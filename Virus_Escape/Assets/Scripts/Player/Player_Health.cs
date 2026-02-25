using UnityEngine;
using UnityEngine.Events;

public class Player_Health : MonoBehaviour,IDamageable
{
    [SerializeField] int maxHealthPoints;


    private void Start()
    {
        UI_Manager.Instance.Health_Display.CurrentHealthPoints = CurrentHealthPoints;
;
}

    private void Update()
    {
        Fall();

        
    }


    public int CurrentHealthPoints
    {
        get
        {
            return maxHealthPoints;
        }
        set
        {
            bool wasAlive = CurrentHealthPoints > 0;
            maxHealthPoints = Mathf.Max(value, 0);
            maxHealthPoints = value;

            if (wasAlive && CurrentHealthPoints <= 0)
            {
                Died.Invoke();

                UI_Manager.Instance.ShowGameOverScreen();
            }
        }
    } 
        public UnityEvent Died;
        public UnityEvent Damaged;

    public bool Alive => CurrentHealthPoints > 0;

    void Fall()
    {
        if (Alive) 
        {
            if (transform.position.y <= -2f)
            {
                Damage(CurrentHealthPoints);

                
            }
        }
        
    }
    public void Damage(int damage)
    {
        if (!Alive) { return; }
        CurrentHealthPoints -= damage;

        UI_Manager.Instance.Health_Display.CurrentHealthPoints = this.CurrentHealthPoints;
        Damaged.Invoke();



    }
}
