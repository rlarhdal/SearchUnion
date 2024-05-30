using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : AnimationController
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsRun = Animator.StringToHash("IsRunning");
    private static readonly int Jumpping = Animator.StringToHash("Jump");

    private readonly float magnituteThreshold = 0.5f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        controller.move += Move;
        controller.run += Run;
        controller.jump += Jump;
    }

    private void Jump()
    {
        animator.SetTrigger(Jumpping);
    }

    private void Run()
    {
        animator.SetBool(IsRun, true);
    }

    private void Move()
    {
        animator.SetBool(IsWalking, true);
    }


}
