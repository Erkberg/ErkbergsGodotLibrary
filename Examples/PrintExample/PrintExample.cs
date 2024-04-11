using Godot;
using System;

namespace ErkbergsGodotLibrary
{
    public partial class PrintExample : Node3D
    {
        public override void _Ready()
        {
            // Simple
            GD.Print("Hi there!");
            GD.Print(Vector3.Up);
            GD.Print(Colors.Chocolate);
            GD.Print(this);

            // Concatenations
            GD.Print("My name is " + Name);
            GD.Print($"My position is {GlobalPosition}");

            GD.PrintT("PrintT", Position, Rotation, Scale);
            GD.PrintS("PrintS", Position, Rotation, Scale);

            // Rich text
            GD.PrintRich("[b]Bold[/b] ", "[i]Italic[/i] ", "[b][i]Bold Italic[/i][/b]");
            // more examples

            // Warnings & Errors
        }
    }
}