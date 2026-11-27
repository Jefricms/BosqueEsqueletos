using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public float invulnerableTime = 3f;

    [Header("Interfaz")]
    public Image[] hearts;       // los 3 círculos
    public Image damageFlash;    // pantalla roja al recibir un golpe
    public Color fullColor = new Color(0.9f, 0.1f, 0.15f, 1f);
    public Color emptyColor = new Color(0.25f, 0.25f, 0.25f, 0.6f);

    int lives;
    bool invulnerable;
    Renderer[] rends;
    public bool IsDead => lives <= 0;

    void Start()
    {
        lives = maxLives;
        rends = GetComponentsInChildren<Renderer>();
        UpdateHearts();
        SetFlash(0f);
    }

    public void TakeDamage()
    {
        if (invulnerable || lives <= 0) return;

        lives--;
        UpdateHearts();
        StartCoroutine(Flash());

        if (lives <= 0)
            Die();
        else
            StartCoroutine(Invulnerability());
    }

    void Die()
    {
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null && agent.isOnNavMesh) agent.isStopped = true;

        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }

    IEnumerator Invulnerability()
    {
        invulnerable = true;
        float t = 0f;
        bool show = true;

        while (t < invulnerableTime)
        {
            show = !show;
            foreach (Renderer r in rends) r.enabled = show;
            yield return new WaitForSeconds(0.15f);
            t += 0.15f;
        }

        foreach (Renderer r in rends) r.enabled = true;
        invulnerable = false;
    }

    IEnumerator Flash()
    {
        if (damageFlash == null) yield break;

        float t = 0f;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            SetFlash(Mathf.Lerp(0.5f, 0f, t / 0.5f));
            yield return null;
        }
        SetFlash(0f);
    }

    void SetFlash(float a)
    {
        if (damageFlash == null) return;
        Color c = damageFlash.color;
        c.a = a;
        damageFlash.color = c;
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
            if (hearts[i] != null)
                hearts[i].color = i < lives ? fullColor : emptyColor;
    }
}