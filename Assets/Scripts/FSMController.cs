using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.FSM;

public class FSMController : MonoBehaviour {

    private FSM fsm;

    private FSMState IdleState;
    private FSMState AimState;
    private FSMState SwingState;
    private FSMState FollowState;

    private MoveAction IdleAction;
    private MoveAction AimAction;
    private SwingAction SwingAction;
    private RotateAction RotateAction;
    private FollowAction FollowAction;

    private Vector3 OverHeadOffSet;
    private Vector3 SideViewOffSet;

    public Camera Camera;
    public GameObject Mallet;
    public GameObject CurrentBall;
    public GameObject Post;

    public static float newMin;
    public static float newMax;

	// Use this for initialization
	void Start () {

        OverHeadOffSet = new Vector3(0, 30, 0);
        SideViewOffSet = new Vector3(10, 0, 0);

        fsm = new FSM("FSM Controller");

        IdleState = fsm.AddState("IdleState");
        AimState = fsm.AddState("AimState");
        SwingState = fsm.AddState("SwingState");
        FollowState = fsm.AddState("FollowState");
        
        IdleAction = new MoveAction(IdleState);
        AimAction = new MoveAction(AimState);
        RotateAction = new RotateAction(AimState);

        SwingAction = new SwingAction(SwingState);
        FollowAction = new FollowAction(FollowState);

        IdleState.AddAction(IdleAction);

        AimState.AddAction(RotateAction);
        AimState.AddAction(AimAction);
        SwingState.AddAction(SwingAction);
        FollowState.AddAction(FollowAction);
        
        IdleState.AddTransition("ToAim", AimState);
        AimState.AddTransition("ToSwing", SwingState);
        SwingState.AddTransition("ToFollow", FollowState);
        FollowState.AddTransition("ToIdle", IdleState);

        IdleAction.Init(Camera.transform, Post.transform, OverHeadOffSet, "ToAim", true);
        RotateAction.Init(Mallet.transform, CurrentBall.transform, "ToSwing", false);
        AimAction.Init(Camera.transform, CurrentBall.transform, OverHeadOffSet, "ToSwing", false);
        SwingAction.Init(Mallet, Camera.transform,"BackSwing" ,"ToFollow");

        FollowAction.Init(Camera.transform, CurrentBall, Mallet, "ToIdle");
        
        fsm.Start("IdleState");

	}
	
	// Update is called once per frame
	void Update () {
        fsm.Update();
        
	}
}
