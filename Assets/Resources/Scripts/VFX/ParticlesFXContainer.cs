using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesFXContainer : MonoBehaviour, IMuzzleVFX
{
    [SerializeField] private bool playOnEnable;
    [SerializeField] private bool playOnStart = true;

    private List<ParticleSystem> _particleList = new List<ParticleSystem>();


    private void OnEnable()
    {
        if(playOnEnable)
            PlayVFX();
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _particleList.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
        }
        if(playOnStart)
            PlayVFX();
    }

    public void PlayVFX()
    {
        foreach (var effect in _particleList)
        {
            effect.Play();
        }
    }
}
