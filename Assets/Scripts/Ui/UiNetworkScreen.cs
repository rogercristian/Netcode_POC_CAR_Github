using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UiNetworkScreen : MonoBehaviour
{
    [SerializeField] private Button startHostBtn;
    [SerializeField] private Button startServerBtn;
    [SerializeField] private Button startClientBtn;

    private void Awake()
    {
        
        startHostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            Hide();
        });

        startServerBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            Hide();
        });

        startClientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }
    //private void Start()
    //{
    //    GameManager.Instance.OnLocalPlayerReadyChange += GameManager_OnLocalPlayerReadyChange; ;

    //}

    //private void GameManager_OnLocalPlayerReadyChange(object sender, System.EventArgs e)
    //{
    //    if (GameManager.Instance.IsLocalPlayerReady())
    //    {
    //        Hide();
    //    }
    //}

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
