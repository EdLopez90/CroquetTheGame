using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.FSM;
public class SwingAction : FSMAction {

    //Swing Action will Anchor the Camera so it is facing the Mallet.
    //The Mallet will first swing, and when the first mouse is clicked, set the back swing
    //The Mallet will then reset, swing in the opposite direction, and set the front swing at the second mouse click
    //Once Back and Front swing are determined, the Mallet will reset once more, 
    //the colliders will activate, and the swing will take place.

    private GameObject mallet;
    private GameObject swingObject;
   

    private Transform camera;
    private Transform anchorPoint;

    private Transform startingPoint; //Holds the starting position and rotatation
    private Transform swingTarget; //Target for the swing motion
    private Vector3 swingVector;
    private Vector3 offset;
    private float startTime;
    private float journeyLength;
    private float speed;

    private Animator anim;
    private AnimatorStateInfo stateInfo;
    private int backSwingHash;
    private int followThroughHash;
    private int fullSwingHash; //might be useless
    private int newMinHash;
    private int newMaxHash;
    private int triggerHash;
    private HingeJoint hinge;


    private string finishEvent;
    private int swingCounter;

    public SwingAction(FSMState owner) : base(owner)
    { }

    public void Init(GameObject mallet, Transform camera, string swingTarget ,string finishEvent)
    {

        this.mallet = mallet;
        this.camera = camera;
        this.finishEvent = finishEvent;
        this.anim = mallet.GetComponentInChildren<Animator>();
        this.hinge = mallet.GetComponentInChildren<HingeJoint>();

        this.followThroughHash = Animator.StringToHash("Base Layer.Follow Through");
        this.backSwingHash = Animator.StringToHash("Base Layer.Back Swing");
        this.fullSwingHash = Animator.StringToHash("Base Layer.Full Swing");
        this.newMaxHash = Animator.StringToHash("NewMaxAngle");
        this.newMinHash = Animator.StringToHash("NewMinAngle");
        this.triggerHash = Animator.StringToHash("Switch State");

        this.offset = new Vector3(0, 4, -2.125f);
        this.swingObject = GameObject.FindGameObjectWithTag(swingTarget);
        this.swingTarget = new GameObject().transform;
        this.startingPoint = new GameObject().transform;
        this.speed = 10.0f;


    }

    public override void OnEnter()
    {
        anim.SetBool("Start", true);
        /* Likely Useless, kept for now
        anchorPoint = GameObject.FindWithTag("Anchor").transform;

        startingPoint.position = mallet.transform.position;
        startingPoint.rotation = mallet.transform.rotation;

        swingTarget.position = swingObject.transform.position;
        swingTarget.rotation = swingObject.transform.rotation;

        startTime = Time.time;
        journeyLength = Vector3.Distance(startingPoint.position, swingTarget.position);
        Debug.Log("Journey Length = " + journeyLength);



         swingVector = mallet.transform.position + new Vector3(0, 10, 2.125f);

        camera.position = Vector3.Slerp(camera.position, anchorPoint.position, 1);
        camera.rotation = Quaternion.LookRotation(mallet.transform.position - camera.position + offset);
        mallet.transform.RotateAround(swingVector, Vector3.forward, 90);
        Time.timeScale = 0;
        
         End of possibly useless*/

    }

    public override void OnUpdate()
    {
        //Grab the current State
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        //Based on the current state, set the new angle for Back Swing
        if (Input.GetButton("Fire1"))
        {
            if (stateInfo.fullPathHash == backSwingHash)
            {
                anim.SetFloat(newMinHash, hinge.angle);
            }

            else if (stateInfo.fullPathHash == followThroughHash)
            {
                anim.SetFloat(newMaxHash, hinge.angle);
            }
            anim.SetTrigger(triggerHash);
        }
    }

    public void Finish()
    {
        if (!string.IsNullOrEmpty(finishEvent))
        {
            GetOwner().SendEvent(finishEvent);
        }
        return;
    }

 

    public override void OnExit()
    {

    }




}
