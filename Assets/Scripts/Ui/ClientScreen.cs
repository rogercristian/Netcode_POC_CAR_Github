using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientScreen : MonoBehaviour
{
   [SerializeField] private Button button;

    private void Awake()
    {
      
       
        button.onClick.AddListener(() =>
        {
            //GameManager.Instance.OnChangeState += GameManager_OnChangeState;
            GameManager.Instance.PlayerReadyScreen();
            Hide();
        });
    }

    //private void GameManager_OnChangeState(object sender, System.EventArgs e)
    //{
        
    //    GameManager.Instance.Teste();
    //}

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}
