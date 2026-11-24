using UnityEngine;

public class ForestTrigger : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public Transform[] spawnPoints;
    bool activated = false;
    GameObject[] skeletons;

    void Start()
    {
        // Se crean al cargar la escena, desactivados
        skeletons = new GameObject[spawnPoints.Length];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            skeletons[i] = Instantiate(skeletonPrefab,
                spawnPoints[i].position, spawnPoints[i].rotation);
            skeletons[i].SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (activated || !other.CompareTag("Player")) return;
        activated = true;

        foreach (GameObject s in skeletons)
            s.SetActive(true);   // su Start() corre aquí y hace la animación de salir del suelo
    }
}