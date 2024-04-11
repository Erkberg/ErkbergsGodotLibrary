using Godot;
using System;

namespace ErkbergsGodotLibrary
{
    public partial class ChangeTimeScaleWithInputAction : Node
    {
        [Export] private string actionName;
        [Export] private float changedTimeScale = 8f;
        [Export] private float regularTimeScale = 1f;

        public override void _Process(double delta)
        {
            if (Input.IsActionJustPressed(actionName))
            {
                Engine.TimeScale = changedTimeScale;
            }
            else if (Input.IsActionJustReleased(actionName))
            {
                Engine.TimeScale = regularTimeScale;
            }
        }
    }
}