using UnityEngine;
using System.Collections;
using UnityEngine.UI;
                                                                       //attached to every planet to gather the collectibles
public class pickableCollector : MonoBehaviour {
    Rigidbody rb;                                                      
    bool check;
    float startTime;
    EllipsoidParticleEmitter myEmitter;                                
    GameObject waterStream;
	// Use this for initialization
	void Start () {

        rb = gameObject.GetComponent<Rigidbody>();
        waterStream = gameObject.transform.FindChild("FirePoint").transform.FindChild("FX_Steam").gameObject;
        myEmitter = waterStream.GetComponent<EllipsoidParticleEmitter>();
    }
    void Update()
    {                                                                 //stop water spray after 6 seconds(can make this public but 6 is best)
        if(Time.time - startTime > 4)
        {
            myEmitter.emit = false;
        }
        if (Time.time - startTime > 6)
        {
            waterStream.SetActive(false);
        }
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="mass")                            //increase the mass of the planet +2 
        {
            rb.mass += 2;
        }
        if(collision.gameObject.tag=="waterspray")
        {
            if (gameObject.tag == "player")
                GameObject.FindGameObjectWithTag("waterbutton").GetComponent<Button>().interactable = true;     //activate the user's spray button for one use     
        }
     }
    

    public void spray()                                                  //start spraying
    {
        startTime = Time.time;
        gameObject.transform.FindChild("FirePoint").transform.FindChild("FX_Steam").gameObject.SetActive(true);
        gameObject.transform.FindChild("FirePoint").transform.FindChild("FX_Steam").gameObject.GetComponent<EllipsoidParticleEmitter>().emit = true;
        GameObject.FindGameObjectWithTag("waterbutton").GetComponent<Button>().interactable = false;
    }
}
