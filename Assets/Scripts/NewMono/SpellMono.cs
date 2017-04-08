using UnityEngine;
using System.Collections;

public class SpellMono : MobaMono {


    /// <summary>
    /// get all particles children and find the max life among them, if anyone is looped then return 10(as long enough value)
    /// </summary>
    /// <returns></returns>
    protected float getMaxParticleLife()
    {
        ParticleSystem[] pses = GetComponentsInChildren<ParticleSystem>();
        float maxLife = 0f;
        bool looped = false;
        foreach (ParticleSystem ps in pses)
        {
            if (ps.loop)
            {
                looped = true;
                break;
            }
            if (ps.duration >= maxLife)
                maxLife = ps.duration;
        }
        if (!looped) 
            return maxLife;
        else
            return 10;
    }
}
