using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class opponentController : MonoBehaviour {
    public AudioClip bumpSound;

    private AudioSource source;
    private Rigidbody rb;
    private float playerDistance, nearestPickable, cliffDistance, playerCliffDitance;                     //for storing the distances from object
    //private Stack<Vector3> task;
    private GameObject[] massPickables, waterPickables;                                                   //pickables currently in arena
    private Ray[] edges;                                                                                  //edges of arena
    private float temp;
    //private MeshFilter planeMesh;                                                                             
    private Vector3 pickablePosition, edgePosition;                                                       
    private List<GameObject> players;
    private GameObject player;
    private bool waterSpray;
  
    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        edges = new Ray[4];
        //task = new Stack<Vector3>();
        //planeMesh = GameObject.Find("Plane").GetComponent<MeshFilter>();
        edges[0].direction = new Vector3(0, 0, -1);                                     //set the edges of our arena
        edges[0].origin = new Vector3(10, 0, 10);
        edges[1].direction = new Vector3(0, 0, 1);
        edges[1].origin = new Vector3(-10, 0, -10);
        edges[2].direction = new Vector3(-1, 0, 0);
        edges[2].origin = new Vector3(10, 0, 10);
        edges[3].direction = new Vector3(1, 0, 0);
        edges[3].origin = new Vector3(-10, 0, -10);
        players = new List<GameObject>(  GameObject.FindGameObjectsWithTag("opponent"));//get all players in the arena
        players.Add(GameObject.FindGameObjectWithTag("player"));
        players.Remove(gameObject);
        waterSpray = false;
        gameObject.transform.position = new Vector3(Random.Range(-9f, 9f), 1f, Random.Range(-9f, 9f)); //Randomly place the opponent in the arena
        source = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "waterspray")                                                  //Got a water spray
            waterSpray = true;

        if (collision.gameObject.tag == "opponent" || collision.gameObject.tag == "player")            //to get an extra bump motion if colling with another player 
        {
            rb.AddForce(-collision.rigidbody.velocity.normalized * 5, ForceMode.Impulse);
            source.PlayOneShot(bumpSound, 1f);
        }


    }
    void OnParticleCollision(GameObject other)                                                        //when attacked by water spray
    {   if(!gameObject.transform.FindChild("FirePoint").transform.FindChild("FX_Steam").gameObject.activeSelf)
        rb.AddForce(other.transform.right * -25);
    }
    
    void Update () {
        if(transform.position.y>1f)
        transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        temp = 100;

        //gameObject.GetComponent<pickableCollector>().spray();                                                               //closest opponent wrt this AI object
        foreach (GameObject a in players)
        {   if (temp > Vector3.Distance(a.transform.position, transform.position))
            {
                playerDistance = Vector3.Distance(a.transform.position, transform.position);
                temp = playerDistance;
                player = a; 
            }  
        }
        gameObject.transform.FindChild("FirePoint").transform.LookAt(player.transform);

        temp = 100;
        massPickables = GameObject.FindGameObjectsWithTag("mass");
        waterPickables = GameObject.FindGameObjectsWithTag("waterspray");
        foreach(GameObject a in massPickables)                                                                                  //nearest pickable
        {
            if(Vector3.Distance(a.transform.position,transform.position)<temp)
            {
                temp = Vector3.Distance(a.transform.position, transform.position);
                pickablePosition = a.transform.position;
            }
        }
        foreach(GameObject a in waterPickables)
        {
            if (Vector3.Distance(a.transform.position, transform.position) < temp)
            {
                temp = Vector3.Distance(a.transform.position, transform.position);
                pickablePosition = a.transform.position;
            }
        }
        nearestPickable = temp;
        temp = 100;
        //Debug.Log(Vector3.Cross(edges[0].direction, transform.position - edges[0].origin).magnitude);                          //nearest edge distance
        foreach(Ray a in edges)
        {
            if(temp > Vector3.Cross(a.direction, transform.position - a.origin).magnitude)
                {
                temp = Vector3.Cross(a.direction, transform.position - a.origin).magnitude;
                
            }
        }
        cliffDistance = temp;
        foreach (Ray a in edges)
        {
            if (temp > Vector3.Cross(a.direction, player.transform.position - a.origin).magnitude)
            {
                temp = Vector3.Cross(a.direction, player.transform.position - a.origin).magnitude;

            }
        }
        playerCliffDitance = temp;
        

        if (playerCliffDitance < cliffDistance)
        {

            if (playerDistance > nearestPickable)                                                                           
            {
                rb.AddForce((pickablePosition - transform.position).normalized * 250 * rb.mass * Time.smoothDeltaTime);      //move to pickable

            }
            else
            {
                if (waterSpray)
                {
                    gameObject.GetComponent<pickableCollector>().spray();                                                    //activate spray when attacking an opponent 
                    waterSpray = false;
                }
                rb.AddForce((player.transform.position - transform.position).normalized * 250 * rb.mass * Time.smoothDeltaTime);//attack the player
            }
        }


        else if(nearestPickable<cliffDistance)
        {
            rb.AddForce((pickablePosition - transform.position).normalized * 250 * rb.mass * Time.smoothDeltaTime);          //move to pickable

        }

        else
        {
            
            Vector3 randPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));

            

            rb.AddForce((randPosition - transform.position).normalized * (400 - (40 * cliffDistance)) * rb.mass * Time.smoothDeltaTime); //try to avoid cliff

            


        }
        
           

        


	}

    

    
}
