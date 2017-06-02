using UnityEngine;
using System.Collections;
/* Generate Pickables*/
public class pickableObjectsGenerator : MonoBehaviour {
    public AudioClip massAudio;                                                  //audio on mass generate
    public AudioClip waterAudio;                                                 //audio on waterspray generate
    GameObject[] pickables;
    float currTime;
    private AudioSource source;
	// Use this for initialization
	void Start () {
        pickables = Resources.LoadAll<GameObject>("Pickables") ;                //load all pickable prefabs
        source = GetComponent<AudioSource>();
        //Debug.Log(pickables.Length);
       
    }
	
	// Update is called once per frame
	void Update () {
        currTime = Time.time;
        if(currTime % 6 < 0.008)                                                   //generate mass pickable every 6 seconds (again could be public)
        {
            source.PlayOneShot(massAudio, 1f);
            GameObject massPickable =  Instantiate<GameObject>(pickables[0]);
            
        }
        if(currTime % 8 <0.008)                                                    //generate water spray pickable every 8 seconds
        {
            source.PlayOneShot(waterAudio, 1f);
            GameObject waterSprayPickable = Instantiate<GameObject>(pickables[1]);
            
        }


	}
}
