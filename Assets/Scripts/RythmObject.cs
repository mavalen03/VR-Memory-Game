using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmObject : MonoBehaviour
{
    //references to the default and active materials
    public Material defaultMaterial;
    public Material litUpMaterial;

    //how long the animation should last and whether it's ongoing
    public float animationDuration;
    public bool animationActive;

    //audio, visual, and game manager references
    private AudioSource audioPlayer;
    private SkinnedMeshRenderer myRenderer;
    private GameManager gameManager;

    //grab a reference to the animator
    private Animator animator;

    void Awake()
    {
        //gather references
        audioPlayer = this.GetComponent<AudioSource>();
        myRenderer = this.GetComponent<SkinnedMeshRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        animator = this.GetComponent<Animator>();

        //assume the default material
        myRenderer.material = defaultMaterial;

        animationActive = false;
    }

    private void OnMouseDown()
    {
        if (gameManager.GetAllowPlayerInput() && !animationActive)
        {
            PlayAnimation();
            gameManager.CompareObject(this);
        }
    }

    //triggers when the player looks at the object
    public void LookedAt()
    {
        //is the player allowed to interact with the object and is its animation inactive?
        if (gameManager.GetAllowPlayerInput() && !animationActive)
        {
            //play the animation and send it to the game manager to check it against the pattern
            PlayAnimation();
            gameManager.CompareObject(this);
        }
    }

    //play the animation
    public void PlayAnimation()
    {
        animationActive = true;
        audioPlayer.Play();
        animator.Play("Clicked");
        //switch to the active material for the duration of the animation
        StartCoroutine(LightUp(animationDuration));
    }

    //switches the object's material to the active one for the given number of seconds
    IEnumerator LightUp(float seconds)
    {
        myRenderer.material = litUpMaterial;
        yield return new WaitForSeconds(seconds);
        myRenderer.material = defaultMaterial;
        animationActive = false;
    }
}
