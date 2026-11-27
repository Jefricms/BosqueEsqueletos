using UnityEngine;

public class Linterna : MonoBehaviour
{
    public Light luz;
    public KeyCode tecla = KeyCode.F;
    public bool empiezaEncendida = true;

    void Start()
    {
        if (luz != null) luz.enabled = empiezaEncendida;
    }

    void Update()
    {
        if (luz != null && Input.GetKeyDown(tecla))
            luz.enabled = !luz.enabled;
    }
}