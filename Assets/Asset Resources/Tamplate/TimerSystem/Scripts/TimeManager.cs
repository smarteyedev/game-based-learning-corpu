using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TimeManager : MonoBehaviour
{
    // Durasi default timer saat pertama kali jalan (dalam detik)
    public float timerDuration = 10f;

    private float currentTime;
    private bool isRunning = false;
    private Coroutine timerCoroutine;
    public UnityEvent OnTimerStart;
    public UnityEvent OnTimerEnd;
    public UnityEvent OnTimerPaused;

    public TMP_Text timerText;

    public float CurrentTime => currentTime;
    public bool IsTimerRunning => isRunning;

    // Buat Memulai timer dari waktu saat ini (tanpa reset)
    public void StartTimer()
    {
        if (isRunning) return;

        Debug.Log("Timer started.");
        isRunning = true;
        OnTimerStart.Invoke();
        timerCoroutine = StartCoroutine(TimerRoutine());
    }

    // Buat ngepause sementara si timer, atau melanjutkan (resume)
    public void TogglePauseResume()
    {
        if (isRunning)
        {
            Debug.Log("Timer paused.");
            isRunning = false;
            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);
            OnTimerPaused.Invoke();
        }
        else
        {
            Debug.Log("Timer resumed.");
            isRunning = true;
            timerCoroutine = StartCoroutine(TimerRoutine());
            OnTimerStart.Invoke(); // Optional: trigger lagi kalo resume
        }
    }

    // Buat Mereset timer dan mulai dari awal, dengan durasi baru jika diberikan
    public void RestartTimer(float newDuration = -1f)
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        if (newDuration > 0)
            timerDuration = newDuration;

        currentTime = timerDuration;
        isRunning = true;
        // Debug.Log("Timer restarted with duration: " + timerDuration);
        OnTimerStart.Invoke();
        timerCoroutine = StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }

        currentTime = 0f;
        UpdateTimerUI();
        isRunning = false;
        timerCoroutine = null;
        Debug.Log("Timer ended.");
        OnTimerEnd.Invoke();
    }

    // Meng-update tampilan waktu di UI Text
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
