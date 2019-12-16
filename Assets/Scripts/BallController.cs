using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject currentBall;
    public GameObject nextBall;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Boundary Line"))
        {
            Debug.Log("Hit");
            this.currentBall.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            currentBall.GetComponent<Rigidbody>().Sleep();
        }
    }

}

