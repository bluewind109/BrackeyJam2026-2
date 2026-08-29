using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public System.Action onTimerComplete;

    [SerializeField] private Image progressBar;

    private float duration;
    private float timer;
    private bool isCountingUp;
    public bool IsPaused { get; private set; }
    public bool IsRunning => timer > 0 && !IsPaused;

    public float GetRemainingTime()
    {
        return timer;
    }

    public void UpdateTime(float timeScale = 1f)
    {
        if (IsPaused) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime * timeScale;
            if (timer <= 0)
            {
                timer = 0;
                onTimerComplete?.Invoke();
            }
        }
        UpdateProgressBar();
    }

    public void Begin(float newDuration, bool isCountingUp = false)
    {
        this.isCountingUp = isCountingUp;
        duration = newDuration;
        timer = duration;
        UpdateProgressBar();
    }

    public void Stop()
    {
        timer = 0;
        UpdateProgressBar();
    }

    public void Pause()
    {
        IsPaused = true;
    }

    public void Resume()
    {
        IsPaused = false;
    }

    private void UpdateProgressBar()
    {
        if (progressBar == null) return;
        if (isCountingUp)
        {
            progressBar.fillAmount = duration > 0 ? 1 - (timer / duration) : 0;
        }
        else
        {
            progressBar.fillAmount = duration > 0 ? timer / duration : 0;
        }
    }
}
