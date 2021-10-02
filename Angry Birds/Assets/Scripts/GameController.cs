using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public SlingShoot slingShoot;
    public TrailController TrailController;
    public List<Birds> Birds;
    public List<Enemy> Enemies;
    public BoxCollider2D TapCollider;
    private Birds _shotBird;

    private bool _isGameEnded = false;
    void Start()
    {
        //setiap burungnya hancur, akan memanggil method ChangeBird sesuai jumlah burung
        for (int i = 0; i < Birds.Count; i++)
        {
            Birds[i].OnBirdDestroyed += ChangeBird;
            Birds[i].OnBirdShot += AssignTrail;
        }

        //cek jumlah musuh dan status game end akan dipanggil ketika babi abis
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;
        }

        slingShoot.InitiateBird(Birds[0]);

        TapCollider.enabled = false;
        _shotBird = Birds[0];
        
    }

    private void AssignTrail(Birds bird)
    {
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;
    }

    //method untuk ganti burung
    //berdasarkan jumlah burung yang sudah ditentukan di game object
    public void ChangeBird()
    {
        TapCollider.enabled = false;
        if (_isGameEnded)
        {
            return;
        }

        Birds.RemoveAt(0);

        if(Birds.Count > 0)
        {
            slingShoot.InitiateBird(Birds[0]);
            _shotBird = Birds[0];
        }
    }

    public void CheckGameEnd(GameObject destroyedEnemy)
    {
        for(int i = 0; i <Enemies.Count; i++)
        {
            if(Enemies[i].gameObject == destroyedEnemy)
            {
                Enemies.RemoveAt(i);
                break;
            }
        }

        if(Enemies.Count == 0)
        {
            _isGameEnded = true;
        }
    }

    private void OnMouseUp()
    {
        if(_shotBird != null)
        {
            _shotBird.OnTap();
        }
    }
}
