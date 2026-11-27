using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SkeletonAI : MonoBehaviour
{
    public float riseTime = 2f;
    public float attackDistance = 1.5f;
    public float chaseSpeed = 4.5f;
    public float repathInterval = 0.2f;
    public float attackCooldown = 1f;

    static readonly int AttackHash = Animator.StringToHash("Attack");
    static readonly int SpeedHash = Animator.StringToHash("Speed");

    NavMeshAgent agent;
    Animator anim;
    Transform player;
    PlayerHealth playerHealth;
    bool ready = false;
    float nextRepath = 0f;
    float nextAttack = 0f;
    float attackDistSqr;

    IEnumerator Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponentInParent<PlayerHealth>();
        attackDistSqr = attackDistance * attackDistance;

        agent.enabled = false;
        Vector3 finalPos = transform.position;
        Vector3 startPos = finalPos + Vector3.down * 2f;
        transform.position = startPos;

        float t = 0;
        while (t < riseTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, finalPos, t / riseTime);
            yield return null;
        }

        agent.enabled = true;
        agent.speed = chaseSpeed;
        ready = true;
    }

    void Update()
    {
        if (!ready) return;

        // Si el jugador ya murió, el esqueleto se detiene
        if (playerHealth != null && playerHealth.IsDead)
        {
            agent.isStopped = true;
            anim.SetBool(AttackHash, false);
            anim.SetFloat(SpeedHash, 0f);
            return;
        }

        float distSqr = (transform.position - player.position).sqrMagnitude;

        if (distSqr > attackDistSqr)
        {
            if (Time.time >= nextRepath)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                anim.SetBool(AttackHash, false);
                nextRepath = Time.time + repathInterval;
            }
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool(AttackHash, true);

            if (Time.time >= nextAttack && playerHealth != null)
            {
                playerHealth.TakeDamage();
                nextAttack = Time.time + attackCooldown;
            }
        }

        anim.SetFloat(SpeedHash, agent.velocity.magnitude);
    }
}