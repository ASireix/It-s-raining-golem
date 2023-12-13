using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FormNames
{
    Human,
    Wolf,
    Bat
}
[System.Serializable]
public class CharacterForms
{
    public FormNames _formName;
    public SpriteRenderer _sprite;
    public string actionName;
    public MorphController morphController;
}
