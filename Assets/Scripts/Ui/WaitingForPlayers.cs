using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForPlayers : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnLocalPlayerReadyChange += GameManager_OnLocalPlayerReadyChange; ;
        GameManager.Instance.OnChangeState += GameManage_OnChangeState;
        Hide();
    }

    private void GameManage_OnChangeState(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStart())
        {
            Hide();
        }
    }

    private void GameManager_OnLocalPlayerReadyChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsLocalPlayerReady())
        {
            Show();
        }
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
