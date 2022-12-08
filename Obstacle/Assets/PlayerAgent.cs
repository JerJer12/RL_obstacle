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
using UnityEditor;

public class PlayerAgent : Agent
{
    public Transform TargetTransform;
    Rigidbody rigidBody;
    private Vector3 startingPosition = new Vector3(-60.0f, 45.652f, -85.0f);


    public override void OnEpisodeBegin()
    {
        transform.localPosition = startingPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
       sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(TargetTransform.localPosition);  
       
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float speed =10f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed;
        //AddReward(-0.1f);
        float distance_max = Vector3.Distance(TargetTransform.localPosition, startingPosition);
        float distance_scaled = Vector3.Distance(TargetTransform.localPosition, transform.localPosition) /distance_max;
        //Debug.Log(distance_scaled);
        // Debug.Log(-distance_scaled);

        AddReward(1f - distance_scaled);
        AddReward(-0.05f);


    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag == "Finish")
        {
            Debug.Log("You finished");
            AddReward(100f);
            EndEpisode();
        }

        if (collision.collider.tag == "wall")
        {
            AddReward(-10f);
           EndEpisode();

        }
        if (collision.collider.tag == "obstacle")
        {
            AddReward(-5f);

        }
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

}
