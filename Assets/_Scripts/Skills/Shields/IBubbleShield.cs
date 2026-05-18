using System;


public interface IBubbleShield
{
    bool IsActive { get; }
    event Action OnTimerEnded;

    void ResetTimer();
    void ReduceTime(float damage);
}
