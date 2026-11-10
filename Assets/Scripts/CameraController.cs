using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;        // HeadPoint
    public PlayerController player; // player
    public float distance = 3f;
    public float sensitivity = 2f;
    public float firstPersonForwardOffset = 0.15f;
    public bool firstPerson = false;

    float yaw;
    float pitch = 20f;
    float currentDist;
    Transform headBone;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yaw = player.transform.eulerAngles.y;
        currentDist = distance;

        foreach (Transform t in player.GetComponentsInChildren<Transform>())
        {
            if (t.name == "mixamorig:Head" || t.name == "Head")
            {
                headBone = t;
                break;
            }
        }

        ApplyView();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            firstPerson = !firstPerson;
            ApplyView();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, firstPerson ? -70f : 5f, 70f);
    }

    void ApplyView()
    {
        player.firstPerson = firstPerson;
    }

    void LateUpdate()
    {
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
        transform.rotation = rot;

        if (firstPerson)
        {
            Vector3 flatForward = Quaternion.Euler(0, yaw, 0) * Vector3.forward;
            transform.position = target.position + flatForward * firstPersonForwardOffset;
        }
        else
        {
            Vector3 dir = -(rot * Vector3.forward);
            float wanted = distance;
            int mask = ~(1 << player.gameObject.layer);

            if (Physics.SphereCast(target.position, 0.25f, dir, out RaycastHit hit,
                                   distance, mask, QueryTriggerInteraction.Ignore))
                wanted = Mathf.Max(0.6f, hit.distance);

            currentDist = Mathf.Lerp(currentDist, wanted, 10f * Time.deltaTime);
            Vector3 pos = target.position + dir * currentDist;

            Terrain terrain = Terrain.activeTerrain;
            if (terrain != null)
            {
                float groundY = terrain.SampleHeight(pos) + terrain.transform.position.y;
                pos.y = Mathf.Max(pos.y, groundY + 0.5f);
            }

            transform.position = pos;
        }

        if (headBone != null)
            headBone.localScale = firstPerson ? Vector3.zero : Vector3.one;
    }
}