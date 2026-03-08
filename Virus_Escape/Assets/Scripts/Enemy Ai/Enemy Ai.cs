using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator animator;

    [Header("Settings")]
    public float detectionRadius = 15f;
    public float attackRange = 2f;
    public float patrolRadius = 20f;
    public float attackCooldown = 2f;
    public float patrolIdleTime = 3f;
    public float rotationSpeed = 7f;
    public float attackDuration = 1.0f;

    [Header("Combat Settings")]
    public int attackDamage = 10;

    [Header("Chase Settings")]
    public float maxChaseDistance = 25f;

    private Vector3 spawnPosition;

    private NavMeshAgent agent;
    private float cooldownTimer;
    private float idleTimer;
    private float attackTimer;

    private Vector3 patrolPoint;
    private bool isPatrolling;
    private bool isIdle;
    private bool isAttacking;

    private Player_Health playerHealth;

    private enum State { Patrol, Chase, Attack }
    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
           

        // Lock spawn position once at start — everything is anchored here
        spawnPosition = transform.position;

        playerHealth = player.GetComponent<Player_Health>();

        SetNewPatrolPoint();
        currentState = State.Patrol;
    }

    void Update()
    {
        // Stop enemy if player is dead
        if (playerHealth != null && !playerHealth.Alive)
        {
            CancelAttack();
            agent.ResetPath();
            currentState = State.Patrol;
            return;
        }

        cooldownTimer -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceFromSpawn = Vector3.Distance(transform.position, spawnPosition);

        // Detection is measured from SPAWN so the enemy can't "discover" the player
        // while roaming far from home
        float playerDistanceFromSpawn = Vector3.Distance(spawnPosition, player.position);

        bool playerInDetectionRange = playerDistanceFromSpawn <= detectionRadius;
        bool playerInChaseRange = distanceFromSpawn < maxChaseDistance;

        // If attacking but player walked out of attack range, cancel
        if (isAttacking && distanceToPlayer > attackRange)
        {
            CancelAttack();
            currentState = State.Chase;
        }

        // Count down attack duration
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
                EndAttack();
        }

        // Only re-evaluate state when not mid-attack
        if (!isAttacking)
        {
            if (distanceToPlayer <= attackRange && cooldownTimer <= 0f && playerInChaseRange)
            {
                currentState = State.Attack;
            }
            else if (playerInDetectionRange && playerInChaseRange)
            {
                // Player is within the spawn-anchored detection bubble AND enemy hasn't
                // wandered too far — chase is valid
                currentState = State.Chase;
            }
            else
            {
                // Player is out of range — cleanly exit attack/chase and return to patrol
                if (currentState == State.Attack || currentState == State.Chase)
                {
                    animator.ResetTrigger("Attack");
                    agent.ResetPath();
                }

                currentState = State.Patrol;
            }
        }

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                ChasePlayer();
                break;

            case State.Attack:
                Attack();
                break;
        }

        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f && !isAttacking);

        if (!isAttacking)
            RotateTowardsMovementDirection();
    }

    void Patrol()
    {
        if (isIdle)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= patrolIdleTime)
            {
                SetNewPatrolPoint();
                idleTimer = 0f;
            }

            return;
        }

        if (!isPatrolling || Vector3.Distance(transform.position, patrolPoint) < 1.5f)
        {
            isIdle = true;
            isPatrolling = false;
            agent.ResetPath();
        }
    }

    void SetNewPatrolPoint()
    {
        // Always sample from spawnPosition so patrol never drifts
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius + spawnPosition;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolPoint = hit.position;
            agent.SetDestination(patrolPoint);
            isPatrolling = true;
            isIdle = false;
        }
        
    }

    void ChasePlayer()
    {
        isIdle = false;
        isPatrolling = false;

        if (agent.isOnNavMesh && player != null)
        {
            agent.SetDestination(player.position);
        }
            
    }

    void Attack()
    {
        if (isAttacking)
            return;

        float distance = Vector3.Distance(transform.position, player.position);
        float distanceFromSpawn = Vector3.Distance(transform.position, spawnPosition);

        // Double-validate before committing to attack
        if (distance > attackRange || distanceFromSpawn >= maxChaseDistance)
        {
            currentState = distanceFromSpawn >= maxChaseDistance ? State.Patrol : State.Chase;
            return;
        }

        isAttacking = true;
        cooldownTimer = attackCooldown;
        attackTimer = attackDuration;

        agent.ResetPath();

        // Face the player on Y-axis only
        Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(lookPos - transform.position),
            Time.deltaTime * rotationSpeed
        );

        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
    }

    public void DealDamage()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            IDamageable damageable = player.GetComponent<IDamageable>();

            if (damageable != null)
                damageable.Damage(attackDamage);
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
        attackTimer = 0f;
    }

    public void CancelAttack()
    {
        isAttacking = false;
        attackTimer = 0f;
        cooldownTimer = attackCooldown;

        animator.ResetTrigger("Attack");

        if (animator.HasState(0, Animator.StringToHash("Walk")))
            animator.CrossFade("Walk", 0.1f);

        if (agent.isOnNavMesh && player != null)
            agent.SetDestination(player.position);
    }

    void RotateTowardsMovementDirection()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        // During edit mode spawnPosition isn't set yet, so fall back to transform.position
        Vector3 origin = Application.isPlaying ? spawnPosition : transform.position;

        // Patrol radius — fixed at spawn
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origin, patrolRadius);

        // Detection radius — fixed at spawn so it matches the actual detection logic
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, detectionRadius);

        // Max chase distance — fixed at spawn
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(origin, maxChaseDistance);

     
    }
}