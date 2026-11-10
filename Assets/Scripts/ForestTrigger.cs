using UnityEngine;

public class ForestTrigger : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public Transform[] spawnPoints;
    bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        if (activated || !other.CompareTag("Player")) return;
        activated = true;

        foreach (Transform p in spawnPoints)
            Instantiate(skeletonPrefab, p.position, p.rotation);
    }
}