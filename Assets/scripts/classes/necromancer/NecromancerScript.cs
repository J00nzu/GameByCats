using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerScript : ClassScript
{
    public GameObject SkellingtonPrefab;
    GameObject Skellington;

    public GameObject smokePoofPrefab;
    public GameObject summoningEffectPrefab;
    GameObject SummoningEffect;

    protected override void LocalPlayerUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(SummonTimer());
        }

    }

    IEnumerator SummonTimer()
    {
        SummoningEffect = Instantiate(summoningEffectPrefab, new Vector3(transform.position.x, transform.position.y, -9f) , Quaternion.identity) as GameObject;
        SummoningEffect.GetComponent<StayOnTopOf>().target = transform;
        float holdTimer = 0;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        while (holdTimer < 2.5f)
        {
            if (!Input.GetMouseButton(1))
            {
                Destroy(SummoningEffect);
                yield break;
            }
            holdTimer += Time.deltaTime;
            rb.velocity = new Vector2(0, 0);
            yield return null;
        }

        Skellington = Instantiate(SkellingtonPrefab, transform.position + transform.right * 2, transform.rotation) as GameObject;
        Instantiate(smokePoofPrefab, Skellington.transform.position, Quaternion.identity);
        Destroy(SummoningEffect);
        yield return null;
    }
}
