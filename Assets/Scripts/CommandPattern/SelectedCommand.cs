using UnityEngine;
using System.Collections.Generic;

public class SelectedCommand : ICommand
{
    private List<Ball> _balls;
    private Vector3 _prevPosition;

    public SelectedCommand(List<Ball> balls, Tube fromTube)
    {
        this._balls = balls;
    }
    public void Execute()
    {
        Debug.Log("Selected Command");
        foreach (var ball in _balls)
        {
            ball.Selected();

        }
    }
    public void Undo()
    {
        foreach (var ball in _balls)
        {
            ball.Deselected();

        }

    }
}