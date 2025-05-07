using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Komponen tambahan untuk testing UI timer (tidak wajib cuman buat testing aja)
public class TimerUIController : MonoBehaviour
{
    public TimeManager timeManager;
    public Button startButton;
    public Button pauseButton;
    public Button restartButton;

    public TMP_InputField durationInput;
    void Start()
    {
        startButton.onClick.AddListener(StartTimerFromUI);
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        restartButton.onClick.AddListener(RestartWithNewDuration);
    }

    // Fungsi untuk tombol Start:
    // Jika input durasi valid maka timer mulai dengan durasi tersebut
    // Jika tidak maka mulai timer dari durasi sebelumnya
    public void StartTimerFromUI()
    {
        if (!timeManager.IsTimerRunning)
        {
            float newDuration;
            if (float.TryParse(durationInput.text, out newDuration) && newDuration > 0)
            {
                Debug.Log("Start from UI with custom duration: " + newDuration);
                timeManager.RestartTimer(newDuration);
            }
            else
            {
                Debug.Log("Start from UI with existing duration.");
                timeManager.StartTimer();
            }
        }
    }

    // Fungsi untuk tombol Pause/Resume
    public void OnPauseButtonClicked()
    {
        timeManager.TogglePauseResume();
    }

    // Fungsi untuk tombol Restart:
    // Jika input durasi valid maka reset pakai durasi tersebut
    // Jika tidak maka reset pakai durasi sebelumnya
    private void RestartWithNewDuration()
    {
        float newDuration;
        if (float.TryParse(durationInput.text, out newDuration) && newDuration > 0)
        {
            Debug.Log("Restart with new duration: " + newDuration);
            timeManager.RestartTimer(newDuration);
        }
        else
        {
            Debug.Log("Restart with default/existing duration.");
            timeManager.RestartTimer();
        }
    }
}
