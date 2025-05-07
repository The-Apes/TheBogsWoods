using System;
using UnityEngine;


public class HurtBox : MonoBehaviour
{
    [NonSerialized] public GameObject Owner;

    private void Awake()
    {
        Owner = transform.root.gameObject;
    }
}
