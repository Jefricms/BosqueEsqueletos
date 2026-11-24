using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SkeletonAI : MonoBehaviour
{
    public float riseTime = 2f;
    public float attackDistance = 1.5f;
    public float chaseSpeed = 4.5f;
    public float repathInterval = 0.2f;   // NUEVO

    // NUEVO: hashes calculados una sola vez
    static readonly int AttackHash = Animator.StringToHash("Attack");
    static readonly int SpeedHash = Animator.StringToHash("Speed");

    NavMeshAgent agent;
    Animator anim;
    Transform player;
    bool ready = false;
    bool caught = false;
    float nextRepath = 0f;                // NUEVO
    float attackDistSqr;                  // NUEVO

    IEnumerator Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player").transform;
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
        if (!ready || caught) return;

        float distSqr = (transform.position - player.position).sqrMagnitude;

        if (distSqr > attackDistSqr)
        {
            // Recalcular ruta solo cada 0.2 s
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
            caught = true;
            Invoke(nameof(Restart), 1.5f);
        }
        anim.SetFloat(SpeedHash, agent.velocity.magnitude);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}