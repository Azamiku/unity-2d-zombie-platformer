using UnityEngine;



[RequireComponent(typeof(Collider2D))]
public class EnemyChaser : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 3f;
    public float detectionRadius = 5f;
    public float stopDistance = 0.6f;

    [Header("Ataque")]
    public int damage = 1;
    public float attackCooldown = 2f;
    public float attackRange = 0.8f;

    private Transform player;
    private Animator animator;
    private float lastAttackTime = -999f;
    private Rigidbody2D playerRb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        GetComponent<Collider2D>().isTrigger = true; // Para no empujar al jugador
    }

    private void Update()
    {
        // Buscar jugador si aún no está asignado
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p)
            {
                player = p.transform;
                playerRb = p.GetComponent<Rigidbody2D>();
            }
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        // Solución para que no persiga a Emy cuando esté en el aire
        bool playerInAir = playerRb != null && Mathf.Abs(playerRb.linearVelocity.y) > 0.01f;

        float dist = Vector2.Distance(transform.position, player.position);

        // Mover hacia el jugador solo si está en suelo y dentro del rango de detección
        if (!playerInAir && dist <= detectionRadius && dist > attackRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * speed * Time.fixedDeltaTime);

            if (animator)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isAttacking", false);
            }

            // Girar según posición del jugador
            Vector3 scale = transform.localScale;
            scale.x = (player.position.x > transform.position.x) ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (dist <= attackRange)
        {
            // Animación de ataque
            if (animator)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", true);
            }
        }
        else
        {
            // Quieto
            if (animator)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isAttacking", false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
