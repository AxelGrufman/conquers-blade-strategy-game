using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class OrderGiver : MonoBehaviour
{
    public enum Orders { Hold, MoveInFormationToTarget, FollowInFormation, Charge, MoveToTarget, AttackAtWill, GuardArea, none}
    public enum FormationTypes { Line, Column, square}
    public bool IsLooseFormation = false;
    public Orders CurrentOrder;
    public FormationTypes CurrentFormation;
    public GameObject TempTarget;
    public GameObject Player;

    public List<GameObject> Units = new List<GameObject>();

    private void Start()
    {
        
    }







    private void Update()
    {
        switch(CurrentOrder)
        {
            case Orders.Hold:
               HoldPosition();
                break;
            case Orders.MoveInFormationToTarget:
                MoveInFormationToTarget(TempTarget.transform.position);
                break;
            case Orders.FollowInFormation:
                FollowInFormation(Player);
                break;
            case Orders.Charge:
                ChargeAtTarget(TempTarget);
                break;
            case Orders.MoveToTarget:
                MoveToTarget(TempTarget.transform.position);
                break;
            case Orders.AttackAtWill:
                AttackAtWill();
                break;
            case Orders.GuardArea:
                GuardArea(TempTarget.transform.position, 10f);
                break;
            case Orders.none:
                // No action
                break;
        }
    }


    private void GiveOrder(Vector3 Target)
    {
        for (int i = 0; i < Units.Count; i++)
        {
           Units[i].GetComponent<Movment>().MoveToTarget(Target);
        }
    }
    private void HoldPosition()
    {
   
    }
    private void MoveInFormationToTarget(Vector3 targetPosition)
    {
        // Implement formation movement logic here
    }
    private void FollowInFormation(GameObject leader)
    {
        // Implement follow formation logic here
    }
    private void ChargeAtTarget(GameObject target)
    {
       GiveOrder(target.transform.position);
    }
    private void MoveToTarget(Vector3 targetPosition)
    {
        GiveOrder(targetPosition);
    }
    private void AttackAtWill()
    {
        // Implement attack at will logic here
    }
    private void GuardArea(Vector3 areaCenter, float radius)
    {
        // Implement guard area logic here

    }
    }
