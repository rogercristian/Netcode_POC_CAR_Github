using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : NetworkBehaviour
{
    private Vector2 moveDirection = Vector2.zero;

    private bool interactPressed = false;
    private bool submitPressed = false;
    private Vector2 acceleratorPressed = Vector2.zero;
    private bool hardBrakePressed = false;

    //   private static InputManager instance;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(this);
        }
    }
    private void Awake()
    {
        //if (instance != null) Debug.LogError(" Já existe um input manager na cena");

        //instance = this;
    }
    //public static InputManager GetInstance() { return instance; }

    /**
     Configuração do controle de direção do jogo 
     */
    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
    }

    /**
     Configuração de input dos botoes 
     */
    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            submitPressed = true;
        }
        else if (context.canceled)
        {
            submitPressed = false;
        }
    }
    public void InteractPressed(InputAction.CallbackContext context)
    {
        if (context.performed) { interactPressed = true; }
        else if (context.canceled) { interactPressed = false; }
    }

    public void AcceleratePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            acceleratorPressed = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            acceleratorPressed = Vector2.zero;

        }
    }

    public void HardBakePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            hardBrakePressed = true;
        }
        else if (context.canceled)
        {
            hardBrakePressed = false;
        }
    }
    /**
     Aqui habilita o uso dos botoes em outros contextos
     */

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }
    public bool GetSubmitPressed()
    {
        bool result = submitPressed;
        submitPressed = false;
        return result;
    }
    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }

    public Vector2 GetAcceleratePressed()
    {
        return acceleratorPressed;
    }

    public bool GetHardBrakePressed()
    {
        bool result = hardBrakePressed;
        //  hardBrakePressed = false;
        return result;
    }

    /***/

    public void RegisterSubmitPressed()
    {
        submitPressed = false;
    }
}
