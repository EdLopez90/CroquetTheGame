using UnityEngine;

public class MalletController : MonoBehaviour {

    public float speed;

    private Rigidbody rb;
    private GameObject currentBall;

    public GameObject Blue;
    public GameObject Red;
    public GameObject Black;
    public GameObject Yellow;

    int floorMask;
    float camRayLength = 1000f;

    private Transform _XForm_Player;
    private Transform _XForm_Parent;

    private Vector3 _LocalRotation;
    private HingeJoint _Hinge_Player;
    private JointMotor _Motor_Player;

    public float MouseSensitivity = 4f;
    public float OrbitDampening = 10f;

    public bool PlayerDisabled = false;

    public ScriptableObject StateController;

    private Transform target;
         
	// Use this for initialization
	void Start () {

        rb = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask ("Ground");
        currentBall = Blue;

        this._Hinge_Player = GetComponent < HingeJoint > ();
        this._Motor_Player = _Hinge_Player.motor;

        this._XForm_Player = this.transform;
        this._XForm_Parent = this.transform.parent;
        this._XForm_Parent.position = currentBall.transform.position;
        
    }

    // Update is called once per frame
    void Update () {


        if (Input.GetKeyDown(KeyCode.LeftShift))

            PlayerDisabled = !PlayerDisabled;

        if (!PlayerDisabled)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit floorHit;

            Physics.Raycast(camRay, out floorHit, camRayLength, floorMask);
            Vector3 parentPos = _XForm_Parent.position;

            //Set Y to zero, avoids problems.
            parentPos.y = 0f;

            Vector3 mousePos = floorHit.point - parentPos;
            mousePos.y = 0f;


            Quaternion QT = Quaternion.LookRotation(mousePos);

            _XForm_Parent.rotation = Quaternion.Slerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);
        }

        Swing();
    }


    void SetNextBall()
    {
        switch (currentBall.name)
        {
            case "Blue":
                currentBall = Red;
                break;
            case "Red":
                currentBall = Black;
                break;
            case "Black":
                currentBall = Yellow;
                break;
            case "Yellow":
                currentBall = Blue;
                break;
                
        }

        _XForm_Parent.position = currentBall.transform.position;
    }

    void Swing()
    {
        if(Input.GetMouseButtonDown(2))
        {
            this.PlayerDisabled = !this.PlayerDisabled;
            _Motor_Player.targetVelocity = -90f;
            _Hinge_Player.useMotor = !this._Hinge_Player.useMotor;
            _Hinge_Player.motor = _Motor_Player;

            
            
        }
    }
}

