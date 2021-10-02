﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShoot : MonoBehaviour
{
    //memasukan circle collider
    public CircleCollider2D Collider;
    private Vector2 _startPos;

    public LineRenderer Trajectory;

    //serialized biar bisa diedit di unity
    //radius = panjang tali ketapel bisa ditarik
    [SerializeField]
    private float _radius = 0.75f;

    //kecepatan lempar
    [SerializeField]
    private float _throwSpeed = 30f;

    //masukin burung biar bisa dimanipulasi disini
    private Birds _bird;


    void Start()
    {
        //inisialisasi nilai posisi mulai
        _startPos = transform.position;
    }

    
    private void OnMouseUp()
    {
        
        Collider.enabled = false;
        
        Vector2 velocity = _startPos - (Vector2)transform.position;
        float distance = Vector2.Distance(_startPos, transform.position);

        //balikin ketapel ke posisi semula
        gameObject.transform.position = _startPos;
        Trajectory.enabled = false;

        //memberikan parameter ke method shoot yang ada di Birds.cs
        _bird.Shoot(velocity, distance, _throwSpeed);
    }

    private void OnMouseDrag()
    {
        //mengubah posisi mouse ke world position
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //hitung supaya karet ketapel berada dalam radius yang ditentukan
        Vector2 dir = p - _startPos;
        if (dir.sqrMagnitude > _radius)
        {
            dir = dir.normalized * _radius;
            transform.position = _startPos + dir;
        }

        if (!Trajectory.enabled)
        {
            Trajectory.enabled = true;
        }

        float distance = Vector2.Distance(_startPos, transform.position);

        DisplayTrajectory(distance);
    }

    void DisplayTrajectory(float distance) 
    {
        if(_bird == null)
        {
            return;
        }

        Vector2 velocity = _startPos - (Vector2)transform.position;

        int segmentCount = 5;
        Vector2[] segments = new Vector2[segmentCount];

        //posisi awal trajectory merupakan posisi mouse dari player saat ini
        segments[0] = transform.position;

        //velocity awal
        Vector2 segVelocity = velocity * _throwSpeed * distance;

        for(int i = 1; i<segmentCount; i++)
        {
            float elapsedTime = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
        }

        Trajectory.positionCount = segmentCount;
        for (int i = 0;i<segmentCount; i++)
        {
            Trajectory.SetPosition(i, segments[i]);
        }
    }

    //perkumpulan burung
    public void InitiateBird(Birds bird)
    {
        _bird = bird;
        _bird.MoveTo(gameObject.transform.position, gameObject);
        Collider.enabled = true;
    }
}
