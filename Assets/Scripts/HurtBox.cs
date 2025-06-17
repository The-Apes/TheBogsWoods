using System;
using Managers;
using UnityEngine;


public class HurtBox : MonoBehaviour
{
    [NonSerialized] public GameObject Owner;
    [SerializeField] private AudioClip hurtSound;

    private void Awake()
    {
        Owner = transform.root.gameObject;
    }

    public void PlayHurtSound()
    {
        AudioManager.instance.PlaySFXAt(hurtSound, transform);
    }
}
