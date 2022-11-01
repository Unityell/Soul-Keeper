using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void InputMessage(string Message);
    public static event InputMessage Signal;
    public static bool IsEnabled;

    public void EmitSignal(string NewMessage)
    {
        Signal?.Invoke(NewMessage);
    }
    public void Initialization()
    {
        IsEnabled = true;
    }
    public void Resume()
    {
        EmitSignal("HidePausePannel");
    }
    public void Menu()
    {
        EmitSignal("Menu");
    }
    public void Restart()
    {
        EmitSignal("Restart");
    }
    public void Continue()
    {
        EmitSignal("Continue");
    }
    public void Shop()
    {
        EmitSignal("Shop");
    }
    public void AddHp()
    {
        EmitSignal("AddHp");
    }
    public void AddEnergy()
    {
        EmitSignal("AddEnergy");
    }

    void Update()
    {
        if(IsEnabled)
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                EmitSignal("Jump");
            }
            if(Input.GetKeyDown(KeyCode.D))
            {
                EmitSignal("Kick");
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                EmitSignal("Ability");
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                EmitSignal("ShowPausePannel");
            }
        }
    }
}
