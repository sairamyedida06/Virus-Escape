using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CrawlerZombie : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private Rigidbody rb;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float detectionRadius = 12f;
    public float rotationSpeed = 8f;

    [Header("Damage")]
    public int contactDamage = 1;

    void Start()
    {
        // Get the Rigidbody component automatically
        rb = GetComponent<Rigidbody>();

        // Setup Rigidbody for top-down/grounded movement
        rb.useGravity = true;
        rb.freezeRotation = true; // Prevents the zombie from tipping over
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Makes movement smooth
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Prevents phasing at speed
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            // We calculate the rotation in Update for smoothness
            Vector3 direction = (player.position - transform.position).normalized;
            RotateTowards(direction);
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            // PHYSICS MOVEMENT: This is what stops the phasing
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Ensure the zombie doesn't try to fly upward

            Vector3 movePosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(movePosition);
        }
    }

    void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        direction.y = 0; // Keep the zombie flat on the ground
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ensure your Player object is tagged "Player" in the Inspector
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(contactDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visual aid to see the detection range in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}