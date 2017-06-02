using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;


public class gameController : MonoBehaviour {
    List<GameObject> players;                                                               //All players - Sphere Gameobjects with tag player or opponent
    GameObject endmenu,player;                                                              //User player with tag player
    public AudioClip background;                                                            //background music
    private AudioSource source;                                                             
    bool paused;                                                                            //to pause and resume game

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;                                                                 //resume if paused
        source = GetComponent<AudioSource>();
        players =  new List<GameObject>(  GameObject.FindGameObjectsWithTag("opponent"));
        endmenu = GameObject.FindGameObjectWithTag("endmenu");
        player = GameObject.FindGameObjectWithTag("player");
        players.Add(player);
        paused = false;
        if(endmenu!=null)
        endmenu.SetActive(false);                                                           // disable end menu from scene: main
        source.playOnAwake = true;

    }
	
	// Update is called once per frame
	void Update () {
	 for(int i = 0; i<players.Count;i++)                                                    //keeping track of alive players
        { 

            if(players[i]!=null &&  players[i].transform.position.y<0)
            {
                
                //players[i].SetActive(false);
                //Destroy(players[i]);
                players.Remove(players[i]);
                if (!players.Contains(player))                                              //When user is kicked out of the arena
                    {
                    GameObject.FindGameObjectWithTag("end").GetComponent<Text>().text = "Try Again :(";     
                    pauseGame();
                    endmenu.SetActive(true);
                    GameObject.FindGameObjectWithTag("pause").SetActive(false);

                }
                if (players.Count == 1)                                                     //When user is Last-man-standing yay!!
                {
                    if(players[0].tag == "player")
                    {
                        GameObject.FindGameObjectWithTag("end").GetComponent<Text>().text = "You Win :D";
                        
                    }
                    
                    pauseGame();
                    endmenu.SetActive(true);
                    GameObject.FindGameObjectWithTag("pause").SetActive(false);

                }

            }
        }

     if(GameObject.Find("brokenglass(Clone)") != null)                                                //Destroy glass pieces to maintain the arena
        {
            Destroy(GameObject.Find("brokenglass(Clone)"), 5);
        }


	}


    public void restartGame()                                                                          //Restart Game
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void pauseGame()                                                                             //Pause-Resume Game
    {
        if (!paused)
        {
            Time.timeScale = 0;

            GameObject.FindGameObjectWithTag("pause").GetComponent<Image>().sprite = Resources.Load<Sprite>("ColorfulButtons/Play");
            paused = !paused;
        }
        else
        {
            Time.timeScale = 1;
            GameObject.FindGameObjectWithTag("pause").GetComponent<Image>().sprite = Resources.Load<Sprite>("ColorfulButtons/Pause");
            paused = !paused;
        }
    }

    public void StartGame()                                                                             //Start the main scene
    {
        SceneManager.LoadScene("main");
    }
    public void quit()                                                                                   //Quit Game
    {
        Application.Quit();
    }
    public void MainMenu()                                                                               //Goto Main Menu
    {
        SceneManager.LoadScene("StartScreen");
    }
}
