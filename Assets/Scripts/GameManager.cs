using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    //the player's score and whether they can interact with the objects
    public int playerScore;
    private int numRightInARow;
    private bool allowPlayerInput;

    //how long the pattern should start and lists to hold the object templates and the current pattern
    public int patternStartingLength;
    public List<RythmObject> objectTemplates = new List<RythmObject>();
    public List<RythmObject> rythmPattern = new List<RythmObject>();


    //references to sounds
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip goSound;
    private AudioSource audioPlayer;

    //the object that the player needs to interact with next and their score
    private RythmObject modelObject;

    //reference to the floating text that guides the player's process and shows their score
    public TextMeshPro floatingText;
    public TextMeshPro scoreText;

    // Start is called before the first frame update
    void Start()
    {
        //find the audio player
        audioPlayer = this.GetComponent<AudioSource>();
        //start a new round
        BeginNewRound();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + playerScore;
    }

    //adds a random object from the list of templates to the pattern
    void addObjectToPattern()
    {
        RythmObject objectToAdd = objectTemplates[Random.Range(0, objectTemplates.Count)];
        if (rythmPattern.Count != 0)
        {
            while(objectToAdd == rythmPattern[rythmPattern.Count - 1])
            {
                objectToAdd = objectTemplates[Random.Range(0, objectTemplates.Count)];
            }
        }
        rythmPattern.Add(objectToAdd);
    }

    //begins a new round with a new pattern
    void BeginNewRound()
    {
        //get rid of any preexisting pattern
        rythmPattern.Clear();
        //add the starting number of objects to the pattern
        for (int i = 0; i < patternStartingLength; i++)
        {
            addObjectToPattern();
        }
        //play the pattern for the player after a 2 second delay
        StartCoroutine(PlayRythmPattern(2));
    }

    //play the pattern for the player
    public IEnumerator PlayRythmPattern(int startDelay)
    {
        //don't let the player interact while playing the patter
        allowPlayerInput = false;

        //wait a certain number of seconds to begin the playback
        yield return new WaitForSeconds(startDelay);

        //set the text
        floatingText.text = "Pay attention!";

        //the first object the player needs to get is the first one in the pattern
        modelObject = rythmPattern[0];
        numRightInARow = 0;
        //loop over the objects
        foreach (RythmObject currentObject in rythmPattern)
        {
            //activate the object
            currentObject.PlayAnimation();

            //wait until the object becomes interactable
            //this signals that it's done with its animation
            yield return new WaitUntil(() => !currentObject.animationActive);

            //wait another fraction of a second so the rhythm doesn't go too fast
            yield return new WaitForSeconds(.2f);
        }

        //add a small delay when the playback is done to avoid the player looking too quickly
        floatingText.text = "Get ready...";
        yield return new WaitForSeconds(2.5f);

        //when the playback is done, let the player interact with the objects
        audioPlayer.clip = goSound;
        audioPlayer.Play();
        floatingText.text = "Go!";
        allowPlayerInput = true;
    }

    //add another item to the pattern and start the playback again
    void ExpandPattern()
    {
        addObjectToPattern();
        StartCoroutine(PlayRythmPattern(3));
    }

    //check if the player's object is the same as the one they need from the pattern
    public void CompareObject(RythmObject submittedObject)
    {
        //is the player's object the same as the model?
        if(submittedObject == modelObject)
        {
            //if so, count how many they've gotten
            numRightInARow++;
            //was that the last object in the rythm?
            if(numRightInARow == rythmPattern.Count)
            {
                //if it was, play the "You Win" sound and add to the pattern
                playerScore++;
                audioPlayer.clip = winSound;
                audioPlayer.Play();
                floatingText.text = "Great!";
                ExpandPattern();
            }
            else
            {
                //if not, move on the next item in the pattern
                modelObject = rythmPattern[numRightInARow];
                return;
            }
        }
        else
        {
            //if the player submitted the wrong object, play the "You Lose" sound and start a new round
            audioPlayer.clip = loseSound;
            audioPlayer.Play();
            playerScore = 0;
            floatingText.text = "Too bad!";
            Invoke("BeginNewRound", 1);
        }
    }

    //return whehter the player can interact with objects
    public bool GetAllowPlayerInput()
    {
        return allowPlayerInput;
    }
}
