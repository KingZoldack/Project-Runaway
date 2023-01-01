using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public static AnimatorManager Instance { get; private set; }

    [SerializeField]
    Animator _playerAnimator;

    string _currentState;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (_currentState == newState)
            return;

        //play the animation
        _playerAnimator.Play(newState);

        //reassign the current state
        _currentState = newState;
    }
}
