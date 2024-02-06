using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnChangeState;
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameEnd

    }

    private State state;

    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 5f;
    private float gamePlayingTimer = 1000f;

    public bool isNight = false;

    public new Light light = new Light();
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Tem outro GameManager aqui! =O ");
        }
        Instance = this;

        state = State.WaitingToStart;
    }
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0)
                {
                    state = State.CountdownToStart;
                    Time.timeScale = 1;
                    OnChangeState?.Invoke(this,EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0)
                {
                    state = State.GamePlaying;
                    Time.timeScale = 3;
                    OnChangeState?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
                {
                    state = State.GameEnd;
                    OnChangeState?.Invoke(this, EventArgs.Empty);
                }
                break;
            
            default:
                state = State.GameEnd;
                break;
        }

        Debug.Log(state);

        if (isNight)
        {
            light.GetComponent<Light>().intensity = 10.0f;
        }
        else
        {
            light.GetComponent<Light>().intensity = 100000.0f;

        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public  bool IsCountdownToStart()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }
}
