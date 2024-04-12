using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class StateMachine : Node
{
    [Export] private State currentState;
    [Export] private Array<State> states;

    public override void _Ready()
    {
        currentState.EnterState();

        foreach (State state in states)
        {
            state.stateMachine = this;
        }
    }

    public void ChangeState<T>()
    {
        State newState = states.Where((state) => state is T).FirstOrDefault();

        if (newState == null)
            return;

        if (currentState is T)
            return;

        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public override void _Process(double delta)
    {
        currentState.ProcessState(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        currentState.PhysicsProcessState(delta);
    }
}
