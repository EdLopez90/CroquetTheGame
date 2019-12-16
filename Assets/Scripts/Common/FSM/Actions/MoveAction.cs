using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.FSM;

public class MoveAction : FSMAction {
    /// <summary>
    /// This action will take a transform, and Lerp it to a desired point based on another Game Object
    /// </summary>
    /// <param name="owner"></param>
    /// 

    private Transform transform;
    private Transform target;

    private Vector3 offset;
    private string finishEvent;
    private bool switchTrigger;

    public MoveAction (FSMState owner) : base (owner)
    { }
    public void Init(Transform transform, Transform target, Vector3 offset, string finishEvent, bool switchTrigger)
    {
        this.transform = transform;
        this.target = target;
        this.offset = offset;
        this.finishEvent = finishEvent;
        this.switchTrigger = switchTrigger;

    }

    public override void OnEnter()
    {
        SetPosition(transform, target);
    }

    public override void OnUpdate()
    {
        //switchTrigger is true during the Aim state, so while you are holding down fire, you can move the mouse and aim.
        if((Input.GetButton("Fire1") == switchTrigger) )
        {
            Finish();
            return;
        }
        
        
    }
    public void SetPosition(Transform transform, Transform target)
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, 100f);
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }
    public void Finish ()
    {
        
        if(!string.IsNullOrEmpty (finishEvent))
        {
            GetOwner().SendEvent(finishEvent);
        }
        return;
    }
}
