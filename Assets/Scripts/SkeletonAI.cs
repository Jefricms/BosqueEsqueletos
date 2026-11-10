using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SkeletonAI : MonoBehaviour
{
    public float riseTime = 2f;
    public float attackDistance = 1.5f;
    public float chaseSpeed = 4.5f;

    NavMeshAgent agent;
    Animator anim;
    Transform player;
    bool ready = false;
    bool caught = false;

    IEnumerator Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player").transform;

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

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetBool("Attack", false);
        }
        else
        {
            agent.isStopped = true;
            anim.SetBool("Attack", true);
            caught = true;
            Invoke(nameof(Restart), 1.5f);
        }
        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}