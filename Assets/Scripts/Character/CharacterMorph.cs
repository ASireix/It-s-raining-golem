using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterMorph : MonoBehaviour
{
    public CharacterForms[] characterForms;
    PlayerController playerController;
    AbilitySystem abilitySystem;
    public CharacterForms currentForm { get; private set; }
    [System.NonSerialized] public UnityEvent OnMorphChange = new UnityEvent();
    [SerializeField] int formIndex;
    // Start is called before the first frame update
    void Start()
    {
        currentForm = characterForms[formIndex];
        playerController = GetComponent<PlayerController>();
        abilitySystem = GetComponent<AbilitySystem>();
        //abilitySystem.SetAbilities(currentForm.morphController.abilities);

    }

    public CharacterForms GetCurrentForm()
    {
        return characterForms[formIndex];
    }

    public void OnMorph(InputAction.CallbackContext context)
    {
        for (int i = 0; i < characterForms.Length; i++)
        {
            if (characterForms[i].actionName == context.action.name)
            {
                SwitchForm(i);
                characterForms[i]._sprite.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                characterForms[i]._sprite.transform.parent.gameObject.SetActive(false);
            }
        }
        Debug.Log(context.action.name);
    }

    public void SwitchForm(int to)
    {
        formIndex = to;
        currentForm = characterForms[formIndex];
        OnMorphChange.Invoke();
    }
}
