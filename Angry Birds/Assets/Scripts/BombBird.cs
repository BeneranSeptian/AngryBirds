using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBird : Birds
{
    [SerializeField]
    public float _bombArea = 10;
    public bool _hasBomb = false;
    

    public void Bomb()
    {
        if (State == BirdState.Thrown || State == BirdState.HitSomething && !_hasBomb )
        {
            Collider.radius = Collider.radius * _bombArea;
            
            _hasBomb = true;
        }
    }

    public override void OnTap()
    {
        Bomb();
    }

    



}
