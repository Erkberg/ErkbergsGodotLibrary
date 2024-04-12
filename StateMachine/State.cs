using Godot;
using System;

public partial class State : Node
{
    public StateMachine stateMachine;

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void ProcessState(double delta) { }
    public virtual void PhysicsProcessState(double delta) { }
}
