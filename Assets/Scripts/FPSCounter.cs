using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public KeyCode tecla = KeyCode.F3;
    bool visible = true;
    float acumulado;
    int frames;
    float fps;

    void Update()
    {
        if (Input.GetKeyDown(tecla)) visible = !visible;

        acumulado += Time.unscaledDeltaTime;
        frames++;

        if (acumulado >= 0.5f)
        {
            fps = frames / acumulado;
            acumulado = 0f;
            frames = 0;
        }
    }

    void OnGUI()
    {
        if (!visible) return;
        GUI.Label(new Rect(10, 10, 200, 30), "FPS: " + Mathf.RoundToInt(fps));
    }
}