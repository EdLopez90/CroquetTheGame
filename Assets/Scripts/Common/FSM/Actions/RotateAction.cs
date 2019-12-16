using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.FSM;

public class RotateAction : FSMAction {

    private Transform mallet;
    private Transform currentBall;
    private bool switchTrigger;
    private Ray camRay;
    private RaycastHit floorHit;
    private string finishEvent;

    private int floorMask;
    public float camRayLength = 1000f;
    public float OrbitDampening = 10f;

    public RotateAction (FSMState owner) : base(owner) { }

    public void Init(Transform mallet, Transform currentBall, string finishEvent, bool switchTrigger)
    {
        this.mallet = mallet;
        this.currentBall = currentBall;
        this.switchTrigger = switchTrigger;
        this.finishEvent = finishEvent;
        floorMask = LayerMask.GetMask("Ground");

    }

    public override void OnEnter()
    {
        //First move the mallet to the ball
        mallet.position = currentBall.position;
        

    }

    public override void OnUpdate()
    {
        if(Input.GetButton("Fire1") == switchTrigger)
        {
            Finish();
            return;
        }

        Rotate();
    }
    public override void OnExit()
    {
        base.OnExit();
    }

    public void Finish()
    {
        if(!string.IsNullOrEmpty (finishEvent))
        {
            GetOwner().SendEvent(finishEvent);
        }
        return;
    }

    public void Rotate()
    {
        camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        Physics.Raycast(camRay, out floorHit, camRayLength, floorMask);

        //Create Vector3s, and set the Y values to 0 to stop the weirdness
        Vector3 malletPos = mallet.position;
        Vector3 mousePos = floorHit.point - malletPos;
        malletPos.y = 0f;
        mousePos.y = 0f;



        Quaternion QT = Quaternion.LookRotation(mousePos);

        mallet.rotation = Quaternion.Slerp(mallet.rotation, QT, Time.deltaTime * OrbitDampening);
    }
}
