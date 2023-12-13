using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/InputReader")]
public class InputReader : ScriptableObject, Controls.IPlayerActions
{

    public event UnityAction<Vector2> moveEvent;
    public event UnityAction firstAbilityEvent;
    public event UnityAction secondAbilityEvent;
    public event UnityAction thirdAbilityEvent;

    Controls controls;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }

        EnablePlayerInput();
    }


    private void OnDisable()
    {
        DisableAllInput();
    }

    public void OnBatForm(InputAction.CallbackContext context)
    {
        
    }

    public void OnFirstAbility(InputAction.CallbackContext context)
    {
        if (firstAbilityEvent != null && context.performed)
        {
            firstAbilityEvent.Invoke();
        }
    }

    public void OnHumanForm(InputAction.CallbackContext context)
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (moveEvent != null)
        {
            moveEvent.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnSecondAbility(InputAction.CallbackContext context)
    {
        if (secondAbilityEvent != null && context.performed)
        {
            secondAbilityEvent.Invoke();
        }
    }

    public void OnThirdAbility(InputAction.CallbackContext context)
    {
        if (thirdAbilityEvent != null && context.performed)
        {
            thirdAbilityEvent.Invoke();
        }   
    }

    public void OnWolfForm(InputAction.CallbackContext context)
    {
        
    }

    public void EnablePlayerInput()
    {
        controls.Player.Enable();
    }

    public void DisableAllInput()
    {
        controls.Player.Disable();
    }
}
