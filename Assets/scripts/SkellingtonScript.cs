using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkellingtonScript : EnemyScript{
    
    override protected IEnumerable<Transform> GetTargets()
    {
        foreach (EnemyScript es in netDog.enemies)
        {
            if(es.tag == "Enemy")
                yield return es.transform;
        }
    }
}
