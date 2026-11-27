using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Paneles")]
    public GameObject gameOverPanel;
    public GameObject winPanel;

    [Header("Textos de tiempo")]
    public TMP_Text tiempoGameOver;
    public TMP_Text tiempoVictoria;

    public string nombreMenu = "MenuPrincipal";

    public bool IsEnded { get; private set; }

    void Awake()
    {
        Instance = this;
        IsEnded = false;
        Time.timeScale = 1f;

#if !UNITY_WEBGL
        // Límite de 60 FPS (en WebGL lo controla el navegador)
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
#endif
    }

    void Update()
    {
        if (!IsEnded) return;
        if (Input.GetKeyDown(KeyCode.R)) Restart();
        if (Input.GetKeyDown(KeyCode.M)) IrAlMenu();
    }

    string TiempoTexto()
    {
        float t = Time.timeSinceLevelLoad;
        int min = (int)(t / 60f);
        int seg = (int)(t % 60f);
        return "Tiempo: " + min.ToString("00") + ":" + seg.ToString("00");
    }

    public void GameOver()
    {
        if (IsEnded) return;
        IsEnded = true;
        if (tiempoGameOver != null) tiempoGameOver.text = TiempoTexto();
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Win()
    {
        if (IsEnded) return;
        IsEnded = true;
        if (tiempoVictoria != null) tiempoVictoria.text = TiempoTexto();
        if (winPanel != null) winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreMenu);
    }
}