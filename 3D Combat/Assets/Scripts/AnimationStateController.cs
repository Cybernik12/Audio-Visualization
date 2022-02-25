using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    int isWalkingHash;
    int isRunningHash;

    bool forwardPressed;

    bool isWalking;
    bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        WalkingAnimation();
        RunningAnimation();
    }

    private void WalkingAnimation()
    {
        // if player presses w key
        if (!isWalking && forwardPressed && !isRunning)
        {
            // then set the isWalking boolean to be true
            animator.SetBool(isWalkingHash, true);
        }

        // if player releases w key
        if (isWalking && !forwardPressed && !isRunning)
        {
            // then set the isWalking boolean to be false
            animator.SetBool(isWalkingHash, false);
        }
    }

    private void RunningAnimation()
    {
        // if player presses w key
        if (!isRunning && forwardPressed)
        {
            // then set the isRunning boolean to be true
            animator.SetBool(isRunningHash, true);
        }

        // if player releases w key
        if (isRunning && !forwardPressed)
        {
            // then set the isRunning boolean to be false
            animator.SetBool(isRunningHash, false);
        }
    }
}
