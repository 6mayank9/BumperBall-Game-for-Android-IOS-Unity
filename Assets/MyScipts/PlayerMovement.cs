using UnityEngine;
using System.Collections;
using CnControls;

/*attached to the user planet*/
public class PlayerMovement : MonoBehaviour {
    public int movementSpeed=10;
    private Rigidbody obj;
    //private Transform objTransform;
    private GameObject[] opponents;
    private float temp;
    private GameObject opponent;
    private Ray[] edges;

    void Start () {
        obj = gameObject.GetComponent<Rigidbody>();
        opponents = GameObject.FindGameObjectsWithTag("opponent");           //all opponents
        edges = new Ray[4];
        edges[0].direction = new Vector3(0, 0, -1);                          //edges of arena
        edges[0].origin = new Vector3(10, 0, 10);
        edges[1].direction = new Vector3(0, 0, 1);
        edges[1].origin = new Vector3(-10, 0, -10);
        edges[2].direction = new Vector3(-1, 0, 0);
        edges[2].origin = new Vector3(10, 0, 10);
        edges[3].direction = new Vector3(1, 0, 0);
        edges[3].origin = new Vector3(-10, 0, -10);
    }
    void OnCollisionEnter(Collision collision)                   
    {
        

        if (collision.gameObject.tag == "opponent" )                        //adding extra bump force
        {
            obj.AddForce(-collision.rigidbody.velocity.normalized * 5, ForceMode.Impulse);
        }


    }
    void OnParticleCollision(GameObject other)                              //when attacked by water spray
    {
        if (!gameObject.transform.FindChild("FirePoint").transform.FindChild("FX_Steam").gameObject.activeSelf)
            obj.AddForce(other.transform.right * -25);
    }
    void Update () {

        temp = 100;
        //Debug.Log(Vector3.Cross(edges[0].direction, transform.position - edges[0].origin).magnitude);                          //nearest edge distance
        foreach (Ray a in edges)
        {
            if (temp > Vector3.Cross(a.direction, transform.position - a.origin).magnitude)
            {
                temp = Vector3.Cross(a.direction, transform.position - a.origin).magnitude;

            }
        }
        if(temp < 2 && Time.timeScale==1)                                                                                          
        {
            Handheld.Vibrate();                                                                                               //Vibrate when near Edge as a warning
            //Debug.Log("vibrate");
        }


        temp = 100;
        if(transform.position.y>1)
        gameObject.transform.position = new Vector3(gameObject.transform.position.x,1f,gameObject.transform.position.z);
        foreach (GameObject a in opponents)
        {
            if (temp > Vector3.Distance(a.transform.position, transform.position))
            {
                temp = Vector3.Distance(a.transform.position, transform.position);
                
                opponent = a;
            }
        }
        transform.FindChild("FirePoint").transform.LookAt(opponent.transform);                                              //adjust water spray to look at the nearest opponent

        Vector3 inputVector = new Vector3(CnInputManager.GetAxis("Horizontal"),0, CnInputManager.GetAxis("Vertical"));       //input from joystick
        obj.AddForce(movementSpeed*inputVector*obj.mass);


        //Keyboard controls for testing purposes

        if (Input.GetKey(KeyCode.RightArrow))
        {
            obj.AddForce(new Vector3(movementSpeed, 0,0)*obj.mass);
        }
    if(Input.GetKey(KeyCode.LeftArrow))
        {
            obj.AddForce(new Vector3(-movementSpeed, 0,0)*obj.mass);
        }
    if(Input.GetKey(KeyCode.UpArrow))
        {
            obj.AddForce(new Vector3(0,0, movementSpeed)*obj.mass);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            obj.AddForce(new Vector3(0,0,-movementSpeed)*obj.mass);
        }

    }
}
