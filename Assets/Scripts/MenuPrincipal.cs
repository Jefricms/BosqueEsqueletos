using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuPrincipal : MonoBehaviour
{
    public string nombreEscenaJuego = "Juego";
    public GameObject panelBotones;
    public GameObject panelControles;
    public GameObject panelDespedida;

    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        MostrarMenu();
    }

    public void Jugar()            { SceneManager.LoadScene(nombreEscenaJuego); }
    public void MostrarMenu()      { Mostrar(true, false, false); }
    public void MostrarControles() { Mostrar(false, true, false); }

    public void Salir()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Mostrar(false, false, true);
#else
        Application.Quit();
#endif
    }

    void Mostrar(bool botones, bool controles, bool despedida)
    {
        panelBotones.SetActive(botones);
        panelControles.SetActive(controles);
        if (panelDespedida != null) panelDespedida.SetActive(despedida);
    }
}