using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{

    public GameObject Trail;
    public Birds TargetBird;

    private List<GameObject> _trails;

    private void Start()
    {
        _trails = new List<GameObject>();
    }

    public void SetBird(Birds bird)
    {
        TargetBird = bird;

        for(int i = 0; i < _trails.Count; i++)
        {
            Destroy(_trails[i].gameObject);
        }
        _trails.Clear();
    }

    public IEnumerator SpawnTrail()
    {
        _trails.Add(Instantiate(Trail, TargetBird.transform.position, Quaternion.identity));

        yield return new WaitForSeconds(0.01f);



        if (TargetBird != null && TargetBird.State != Birds.BirdState.HitSomething)
        {
            
            StartCoroutine(SpawnTrail());
            yield return new WaitForSeconds(0.01f);
        }
    }
}
