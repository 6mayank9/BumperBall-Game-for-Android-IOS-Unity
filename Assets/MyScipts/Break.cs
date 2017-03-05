using UnityEngine;
using System.Collections;

public class Break : MonoBehaviour 
{
	public Transform brokenObject;                                                 //import broken glass 
	public float magnitudeCol, radius, power, upwards;                             //set the break point and explosion power
    public AudioClip breakSound;
    private AudioSource source;

    void Start()
    {
        source=GetComponent<AudioSource>();
    }

	void OnCollisionEnter(Collision collision)
	{
        
        if (collision.relativeVelocity.magnitude * collision.gameObject.GetComponent<Rigidbody>().mass > magnitudeCol) //When planet hits with enough magnitude (collecting mass makes it easier)
		{
            source.PlayOneShot(breakSound, 0.3f);                                                                      
            Destroy(gameObject,breakSound.length);                                                                     //replace original glass with broken glass (little delay for playing audio)
            
			GameObject brokenGlasses = Instantiate(brokenObject, transform.position, transform.rotation) as GameObject;
            
			brokenObject.localScale = transform.localScale;
			Vector3 explosionPos = transform.position;
			Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);

			foreach (Collider hit in colliders)                                                               
			{
				if (hit.GetComponent<Rigidbody>())
				{
					hit.GetComponent<Rigidbody>().AddExplosionForce(power*collision.relativeVelocity.magnitude*collision.gameObject.GetComponent<Rigidbody>().mass, explosionPos, radius, upwards);

                   // Destroy(GameObject.FindGameObjectWithTag("broken"),5);
                    
                }
                
			}
            
		}
	}
}
