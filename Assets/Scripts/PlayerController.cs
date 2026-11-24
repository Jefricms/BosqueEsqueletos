using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 1.5f;
    public float runSpeed = 5f;
    public Camera cam;               // si está vacío usa la Main Camera
    public GameObject clickMarker;   // opcional: marcador en el suelo
    public bool firstPerson = false; // lo usa CameraController

    NavMeshAgent agent;
    Animator anim;
    bool running;

    public bool IsMoving
    {
        get { return agent != null && agent.velocity.sqrMagnitude > 0.05f; }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        if (cam == null) cam = Camera.main;

        agent.angularSpeed = 540f;
        agent.acceleration = 20f;
        agent.stoppingDistance = 0.2f;
        agent.autoBraking = true;

        if (clickMarker != null) clickMarker.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryMoveToClick();

        // Ocultar el marcador al llegar
        if (clickMarker != null && clickMarker.activeSelf &&
            !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
            clickMarker.SetActive(false);

        // Animación: 0 = quieto, 0.5 = caminar, 1 = correr
        float animValue = 0f;
        if (IsMoving) animValue = running ? 1f : 0.5f;
        anim.SetFloat("Speed", animValue, 0.1f, Time.deltaTime);
    }

    void TryMoveToClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 300f, ~0, QueryTriggerInteraction.Ignore);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit h in hits)
        {
            if (h.transform.IsChildOf(transform)) continue; // ignora a la propia chica

            if (NavMesh.SamplePosition(h.point, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
            {
                running = Input.GetKey(KeyCode.LeftShift);
                agent.speed = running ? runSpeed : walkSpeed;
                agent.isStopped = false;
                agent.SetDestination(navHit.position);

                if (clickMarker != null)
                {
                    clickMarker.SetActive(true);
                    clickMarker.transform.position = navHit.position + Vector3.up * 0.05f;
                }
            }
            break;
        }
    }
}