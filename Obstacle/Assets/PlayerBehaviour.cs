using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Net;
using System.Transactions;
using System.Security.AccessControl;
using System;
using System.Threading.Tasks;


public class PlayerBehaviour : Agent
{
    private enum ACTIONS
    {
        LEFT = 0,
        FORWARD = 1,
        RIGHT = 2,
        BACK  = 3
    }

    Rigidbody rigidBody;
    public float speed = 10;
    public float force = 10;

    public Transform TargetTransform;
    private const float MAX_DISTANCE = 60f;

    private Vector3 startingPosition = new Vector3(-60.0f, 45.652f, -85.0f);



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startingPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // We don't need this function now because we use the RayPerceptionSensor
        // Note however that we could add additional observations here, if we wanted to, like the speed & velocity of the agent etc.
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;

        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;

        if (Input.GetKey("a"))
        {
            actions[0] = (int)ACTIONS.LEFT;
            //discreteActionsOut[0] = 0;
        }
        else if (Input.GetKey("d"))
        {
            actions[0] = (int)ACTIONS.RIGHT;
            //discreteActionsOut[0] = 2;
        }
        else if (Input.GetKey("w"))
        {

            actions[0] = (int)ACTIONS.FORWARD;
            //discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey("s"))
        {

            //discreteActionsOut[0] = 3;
             actions[0] = (int)ACTIONS.BACK;
        }
    }
   /* public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];

        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
            case 5:
                dirToGo = transform.right * -0.75f;
                break;
            case 6:
                dirToGo = transform.right * 0.75f;
                break;
        }
        /*transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        m_AgentRb.AddForce(dirToGo * m_PushBlockSettings.agentRunSpeed,
            ForceMode.VelocityChange);
    }*/

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.DiscreteActions[0];

       // MoveAgent(actions.DiscreteActions);
       // AddReward(-1f / MaxStep);

         switch (actionTaken)
         {
             case (int)ACTIONS.LEFT:
                 rigidBody.AddForce(Vector3.left * force);
                 break;
             case (int)ACTIONS.RIGHT:
                 rigidBody.AddForce(Vector3.right * force);
                 break;
             case (int)ACTIONS.FORWARD:
                 rigidBody.AddForce(Vector3.forward * force);
                 break;
             case (int)ACTIONS.BACK:
                 rigidBody.AddForce(Vector3.back * force);
                 break;

         }
        float distance_scaled = Vector3.Distance(TargetTransform.localPosition, transform.localPosition) / MAX_DISTANCE;
        Debug.Log(distance_scaled);
       // Debug.Log(-distance_scaled);

        AddReward(1f - distance_scaled ); // [0, 0.1]
        AddReward(-0.01f);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Finish")
        {
            Debug.Log("You finished");
            AddReward(1000f);
            EndEpisode();
        }

        if (collision.collider.tag == "wall")
        {
            AddReward(-2f);
            
        }
        if (collision.collider.tag == "obstacle")
        {
            AddReward(-0.5f);

        }
    }



    // Update is called once per frame
   /* void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            //transform.Translate(Vector3.forward * speed * Time.deltaTime);
             rigidBody.AddForce( force * Vector3.forward );
        }
        if (Input.GetKey("d"))
        {
            //transform.Translate(Vector3.right * speed * Time.deltaTime);
             rigidBody.AddForce(Vector3.right * force);
        }
        if (Input.GetKey("a"))
        {
           // transform.Translate(Vector3.left * speed * Time.deltaTime);
             rigidBody.AddForce(Vector3.left *force);
        }
        if (Input.GetKey("s")) { 
         //transform.Translate(Vector3.back * speed * Time.deltaTime);
        rigidBody.AddForce(Vector3.back * force);
         }
        
    }*/
}

