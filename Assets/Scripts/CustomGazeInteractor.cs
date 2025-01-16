using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

public class CustomGazeInteractor : MonoBehaviour
{
    //references
    private XRRayInteractor gazeInteractor;

    //references and configurations for the eye tracking cursor
    public GameObject cursor;
    public Camera mainCamera;
    public float cursorDistance;
    public bool showCursor;

    void Awake()
    {
        // Get the Gaze Interactor component
        gazeInteractor = GetComponent<XRRayInteractor>();

        showCursor = true;
    }

    void FixedUpdate()
    {
        if (gazeInteractor != null && showCursor)
        {
            // Calculate the position along the ray from the gaze interactor
            Vector3 rayOrigin = gazeInteractor.transform.position;
            Vector3 rayDirection = gazeInteractor.transform.forward;

            // Set the cursor's position a specified distance along the ray
            cursor.transform.position = rayOrigin + rayDirection * cursorDistance;

            // Calculate the direction vector to the target from the main camera
            Vector3 directionToTarget = cursor.transform.position - mainCamera.transform.position;

            // Calculate the rotation using LookRotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Apply the rotation to the cursor
            cursor.transform.rotation = targetRotation;
        }
        cursor.SetActive(showCursor);
    }

    void OnEnable()
    {
        // Subscribe to the Hover Enter event
        gazeInteractor.hoverEntered.AddListener(OnHoverEnter);
        gazeInteractor.hoverExited.AddListener(OnHoverExit);
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        gazeInteractor.hoverEntered.RemoveListener(OnHoverEnter);
        gazeInteractor.hoverExited.RemoveListener(OnHoverExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        //if this method is called, the player is looking at a hovered object
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        //if this method is being called, than the player is no longer looking at a hovered object
        
    }
}
