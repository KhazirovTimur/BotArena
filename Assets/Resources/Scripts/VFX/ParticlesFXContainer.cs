using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesFXContainer : MonoBehaviour
{
    [SerializeField] private bool playOnEnable;

    private List<ParticleSystem> _particleList = new List<ParticleSystem>();


    private void OnEnable()
    {
        if(playOnEnable)
            PlayVFX();
    }

    private void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
            _particleList.Add(transform.GetChild(i).GetComponent<ParticleSystem>());
        PlayVFX();
    }

    public void PlayVFX()
    {
        for(int i = 0; i < _particleList.Count; i++)
            _particleList[i].Play();
    }
}
