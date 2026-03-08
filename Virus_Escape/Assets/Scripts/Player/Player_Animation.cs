using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Player player;

    void Update()
    {
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        bool jump = false;
        bool fall = false;

        if (!player.Grounded)
        {
            if (player.verticalVelocity > 0f)
            {
                jump = true;
            }
            else if (player.verticalVelocity < 0f)
            {
                fall = true;
            }
        }

        Vector3 velocity = player.characterController.velocity;
        velocity.y = 0f;

        float speed = velocity.magnitude;

        animator.SetFloat("Speed", speed);
        animator.SetBool("Jump", jump);
        animator.SetBool("Fall", fall);
    }

    public void OnDeath()
    {
        player.InputHandling = false;
        animator.SetBool("Alive",false);
    }

}