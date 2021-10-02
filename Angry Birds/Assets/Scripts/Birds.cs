using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Birds : MonoBehaviour
{
    //beberapa variable yang akan diedit dari unity
    public enum BirdState { Idle, Thrown, HitSomething};
    public GameObject Parent;
    public Rigidbody2D rigidBody;
    public CircleCollider2D Collider;

    //inisialisasi beberapa variable yang akan diedit dari script ini
    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Birds> OnBirdShot = delegate { };

    //supaya bisa diakses di script lain
    public BirdState State { get { return _state; } }
    //setting rigidbody, collider dan keadaan pertama burung
    private void Start()
    {
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;

    }

    private void FixedUpdate()
    {
        //mengubah keadaan burung dari idle ke thrown berdasarkan kecepatan
        if(_state == BirdState.Idle && rigidBody.velocity.sqrMagnitude >= _minVelocity)
        {
            _state = BirdState.Thrown;
        }

        if((_state == BirdState.Thrown || _state == BirdState.HitSomething) && rigidBody.velocity.sqrMagnitude < _minVelocity &&
            !_flagDestroy)
        {
            //hancurkan game object setelah 2 detik
            // jjika kecepatannya sudah kurang dari batas minimum
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    //fungsi untuk menunda burungnya hancur
    private IEnumerator DestroyAfter(float second)
    {
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    //inisiasi posisi burung biar ga salah kiblat
    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    
    public void Shoot (Vector2 velocity, float distance, float speed)
    {
        Collider.enabled = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.velocity = velocity * speed * distance;
        OnBirdShot(this);
    }

    private void OnDestroy()
    {
        if(_state == BirdState.Thrown || _state == BirdState.HitSomething)
        OnBirdDestroyed();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _state = BirdState.HitSomething;
    }

    public virtual void OnTap()
    {

    }

   



}
