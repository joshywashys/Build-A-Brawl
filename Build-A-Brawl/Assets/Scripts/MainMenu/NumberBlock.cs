using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBlock : MonoBehaviour
{
    private RoundManager rm;
    private ParticleSystem ps;

    
    private IEnumerator PlayParticles()
    {
        yield return new WaitForSeconds(ps.main.duration);
        Destroy(gameObject);
    }

    public void SelfDestruct()
    {
        //do particles
        //ps.Play();
        Destroy(gameObject);
    }

    private void Start()
    {
        rm = FindObjectOfType<RoundManager>();
        //ps = GetComponentInChildren<ParticleSystem>();
        rm.onCancelStart.AddListener(SelfDestruct);
        //find round manager
        //subscribe to cancel event
    }
}
