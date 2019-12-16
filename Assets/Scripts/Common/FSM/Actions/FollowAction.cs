using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.FSM;

public class FollowAction : FSMAction {

    private Transform target;
    private Transform transform;
    private GameObject currentBall;
    private GameObject mallet;
    private GameObject mReset;

    private Vector3 offset;

    private string finishEvent;
    private float timer;

    public FollowAction(FSMState owner) : base (owner)
    { }

    public void Init(Transform transform, GameObject currentBall, GameObject mallet ,string finishEvent)
    {
        
        this.transform = transform;
        this.currentBall = currentBall;
        this.target = currentBall.transform;
        this.finishEvent = finishEvent;
        this.mallet = mallet;
        this.offset = new Vector3(5, 5, 0);
        this.timer = 1;

    }

    public override void OnEnter()
    {
        Debug.Log("Starting to follow");
        transform.position = Vector3.Lerp(transform.position, mallet.transform.position + offset, 100f);
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    public override void OnUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, 100f);

        if((currentBall.GetComponent<Rigidbody>().velocity == Vector3.zero) && timer <= 0)
        {
            Finish();
        }

        timer -= Time.deltaTime;
        
    }

    public void Finish()
    {
        if(!string.IsNullOrEmpty (finishEvent))
        {
            GetOwner().SendEvent(finishEvent);
        }
        return;
    }

    public override void OnExit()
    {
        
    }

}
