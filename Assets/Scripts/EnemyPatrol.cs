using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    public float attackRange = 1.5f; // Distancia a la que atacará
    public int damage = 1;
    public float attackCooldown = 2f; // Tiempo entre ataques

    private Transform targetPoint;
    private Animator animator;
    private Transform player;
    private float lastAttackTime = -999f;

    void Start()
    {
        targetPoint = pointB;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Activar animación de ataque si está cerca
        if (distanceToPlayer < attackRange)
        {
            if (animator != null)
            {
                animator.SetBool("isAttacking", true);
                animator.SetBool("isWalking", false);
            }

            // Aplicar daño con cooldown
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                var playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                    playerHealth.TakeDamage(damage);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isAttacking", false);
                animator.SetBool("isWalking", true);
            }
        }

        // Movimiento de patrulla entre A y B aunque ataque
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
