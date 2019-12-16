using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.FSM;

public class CameraAction : FSMAction {

    private Transform transform;
    private Vector3 positionFrom;
    private Vector3 positionTo;
    private GameObject followObject;
    private bool switchState;
    private Input switchTrigger;

	public CameraAction (FSMState owner) : base (owner)
    {

    }

    public void Init (Transform transform, Vector3 positionFrom, Vector3 positionTo,GameObject followObject, Input switchTrigger)
    {
        this.transform = transform;
        this.positionFrom = positionFrom;
        this.positionTo = positionTo;
        this.followObject = followObject;
        this.switchTrigger = switchTrigger;
    }

    public override void OnEnter()
    {
        if (positionFrom == positionTo)
        {
            return;
        }

        SetPosition(this.positionFrom);
    }

    public override void OnUpdate()
    {
        if (positionFrom == positionTo)
        {
            return;
        }

        SetPosition(Vector3.Lerp(this.positionFrom, this.positionTo, 1f));
    }


    private void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }
}
