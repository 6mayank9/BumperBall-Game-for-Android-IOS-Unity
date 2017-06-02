using UnityEngine;
using System.Collections;
/* attached to every pickable objects*/                                                       
public class pickableObject : MonoBehaviour {
    Vector3 pos;
                               
    // Use this for initialization
	void Start()
    {
        transform.position = new Vector3(Random.Range(-9, 9), 1f, Random.Range(-9, 9));         //Random postion on arena
        pos = transform.position;
    }
    // Update is called once per frame
    void Update () {
        transform.Rotate(new Vector3(1, 0, 1), 2);
        transform.position = pos;

	}

    
    void OnCollisionEnter()                                                                     //Destroy on collision i.e object is picked up
    {
        Destroy(gameObject);
    }

}
