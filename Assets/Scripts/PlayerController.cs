using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2.5f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10f;
    public float gravity = -20f;
    public Transform cameraTransform;
    public bool firstPerson = false;

    CharacterController cc;
    Animator anim;
    float verticalVelocity;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0, v).normalized;

        bool running = Input.GetKey(KeyCode.LeftShift);
        float speed = running ? runSpeed : walkSpeed;

        Vector3 camForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(cameraTransform.right, new Vector3(1, 0, 1)).normalized;
        Vector3 move = camForward * input.z + camRight * input.x;

        if (firstPerson)
        {
            transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        }
        else if (move.magnitude > 0.1f)
        {
            Quaternion target = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }

        if (cc.isGrounded && verticalVelocity < 0) verticalVelocity = -2f;
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * speed;
        velocity.y = verticalVelocity;
        cc.Move(velocity * Time.deltaTime);

        float animValue = move.magnitude < 0.1f ? 0f : (running ? 1f : 0.5f);
        anim.SetFloat("Speed", animValue, 0.1f, Time.deltaTime);
    }
}