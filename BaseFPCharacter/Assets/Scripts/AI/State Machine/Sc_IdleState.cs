using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sc_IdleState : Sc_AIBaseState
{
    private Sc_AIStateManager stateManager;
    private Sc_Player_Movement playerMovementScript;

    private float idleTimer, visionRange, visionConeAngle, audioRange;

    Vector3 randomLookDirection;

    public override void EnterState(float speed, bool playerSeen) {
        stateManager.StartCoroutine(IdleTimed());
    }

    public override void UpdateState(float distPlayer, float angleToPlayer) {
        CanSeePlayer(distPlayer, angleToPlayer);
    }

    public void IdleStartStateInfo(Sc_AIStateManager stateManager, Sc_Player_Movement playerMovementScript, float idleTime, float distRange, float visionAngleRange, float audioDist)
    {
        this.stateManager = stateManager;
        this.playerMovementScript = playerMovementScript;
        idleTimer = idleTime;
        visionRange = distRange;
        visionConeAngle = visionAngleRange;
        audioRange = audioDist;
    }

    public void CanSeePlayer(float distPlayer, float angleToPlayer)
    {
        bool playerHidden = playerMovementScript.ReturnIsHidden();
        if ((distPlayer <= visionRange - 5 && angleToPlayer <= visionConeAngle - 5) && !playerHidden)
        {
            stateManager.playerNoticed = true;
            stateManager.SwitchState(stateManager.aggressionDesicionState);
        }
    }

    public void HeardGunShots(float distPlayer)
    {
        if(distPlayer <= audioRange)
        {

        }

    }

    IEnumerator IdleTimed()
    {
        stateManager.SetIsIdling(true);
        yield return new WaitForSeconds(idleTimer / 3);
        randomLookDirection.x = Random.Range(0, 360);
        randomLookDirection.z = Random.Range(0, 360);
        stateManager.transform.LookAt(randomLookDirection);
        yield return new WaitForSeconds(idleTimer / 3);
        randomLookDirection.x = Random.Range(0, 360);
        randomLookDirection.z = Random.Range(0, 360);
        stateManager.transform.LookAt(randomLookDirection);
        yield return new WaitForSeconds(idleTimer / 3);
        stateManager.SwitchState(stateManager.patrolState);
        stateManager.SetIsIdling(false);
        yield return null;
    }
}
