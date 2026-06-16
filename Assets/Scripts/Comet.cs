using System.Collections.Generic;
using UnityEngine;

public class Comet : MovingAsteroid
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out StaticAsteroid Asteroid)) KillDebrisInTheWay(Asteroid);
        base.OnTriggerEnter(other);

    }

    protected override void UIWarningText() => EventBus.Act(new WarningTextEvent(UITextInfo.CometText ,this));

    private void KillDebrisInTheWay(StaticAsteroid asteroid)
    {
        if (asteroid is not Comet)
        {
            asteroid.KillAsteroid();
        }
    }
    

  
}
