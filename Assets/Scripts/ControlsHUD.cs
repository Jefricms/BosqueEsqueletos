using UnityEngine;

public class ControlsHUD : MonoBehaviour
{
    public GameObject panel;  // PanelControles
    public GameObject hint;   // PistaControles

    void Start()
    {
        Apply(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            Apply(!panel.activeSelf);
    }

    void Apply(bool show)
    {
        panel.SetActive(show);
        if (hint != null) hint.SetActive(!show);
    }
}