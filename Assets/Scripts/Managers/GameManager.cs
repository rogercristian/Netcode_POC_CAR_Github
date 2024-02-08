using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnChangeState;
    public event EventHandler OnLocalPlayerReadyChange;
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameEnd

    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private bool isLocalPlayerReady;
    // private NetworkVariable<float> waitingToStartTimer = new NetworkVariable<float>(1f);
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(5f);
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(1000f);
    private Dictionary<ulong, bool> playerReadyDictionary;

    public bool isNight = false;

    public new Light light = new Light();


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Tem outro GameManager aqui! =O ");
        }
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
    }
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnChangeState?.Invoke(this, EventArgs.Empty);
    }
    public void PlayerReadyScreen()
    {
        if (state.Value == State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            SetPlayerReadyServerRPC();
            OnLocalPlayerReadyChange?.Invoke(this, EventArgs.Empty);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRPC(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool isAllClientReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                isAllClientReady = false;
                break;
            }
           
        }
        if (isAllClientReady)
        {
            state.Value = State.CountdownToStart;
        }
        Debug.Log("Todos o clients tao prontos :" + isAllClientReady);

    }
    private void Update()
    {
        if (!IsServer) { return; }

        switch (state.Value)
        {
            case State.WaitingToStart:
                Time.timeScale = 1;
                break;

            case State.CountdownToStart:
                countdownToStartTimer.Value -= Time.deltaTime;
                if (countdownToStartTimer.Value < 0)
                {
                    state.Value = State.GamePlaying;
                    Time.timeScale = 3;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value < 0)
                {
                    state.Value = State.GameEnd;
                }
                break;

            default:
                state.Value = State.GameEnd;
                break;
        }

        // Debug.Log(state);

        if (isNight)
        {
            light.GetComponent<Light>().intensity = 10.0f;
        }
        else
        {
            light.GetComponent<Light>().intensity = 100000.0f;

        }
    }


    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }
    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsCountdownToStart()
    {
        return state.Value == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer.Value;
    }
}
