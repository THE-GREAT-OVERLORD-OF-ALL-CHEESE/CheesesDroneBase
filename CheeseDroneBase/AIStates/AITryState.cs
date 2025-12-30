namespace CheeseMods.AIHelicopterGunner.AIStates;

public abstract class AITryState
{
    public abstract string Name { get; }
    public abstract float WarmUp { get; }
    public abstract float CoolDown { get; }

    public abstract bool CanStart();
    public abstract void StartState();
    public abstract void UpdateState();
    public abstract bool IsOver();
    public abstract void EndState();
}
