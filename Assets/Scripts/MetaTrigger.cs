using UnityEngine;

public class MetaTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance != null)
            GameManager.Instance.Win();
    }
}