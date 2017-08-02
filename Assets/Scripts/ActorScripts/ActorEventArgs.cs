using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class ActorEventArgs : EventArgs
{
    public ActorScript Sender { get; protected set; }
}

/// <summary>
/// Actor registered an attack - event fires before the damage and effects are dealt.
/// </summary>
class BeingAttackedEventArgs : ActorEventArgs
{    
    public bool IsCritical { get; private set; }
    public Attack TheAttack { get; private set; }
}

class GotHitEventArgs : ActorEventArgs
{
    public BeingAttackedEventArgs Args { get; private set; }

    public GotHitEventArgs(BeingAttackedEventArgs args)
    {
        Args = args;
    }
}

class ReloadStartedEventArgs : ActorEventArgs
{
    public float BaseReloadTime;
}

class DuringReloadEventArgs : ActorEventArgs
{

}

class ReloadFinishedEventArgs : ActorEventArgs
{

}

class DieEventArgs : ActorEventArgs
{

}

class EnemyKilled : ActorEventArgs
{
    public ActorScript Killer { get; private set; }    
    public float ExpReward { get; private set; }
}