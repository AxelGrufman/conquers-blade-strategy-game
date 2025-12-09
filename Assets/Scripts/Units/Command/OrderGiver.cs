using System.Collections.Generic;
using UnityEngine;

public class OrderGiver : MonoBehaviour
{
    public enum Orders { Hold, MoveInFormationToTarget, FollowInFormation, Charge, MoveToTarget, AttackAtWill, GuardArea, none }
    public enum FormationTypes { Line, Column, square }

    public bool IsLooseFormation = false;
    public Orders CurrentOrder;
    public FormationTypes CurrentFormation;

    public GameObject TempTarget;
    public GameObject Player;

    public int LineWidth = 10;
    public int ColumnWidth = 3;

    public GameObject TempUnitParent;

    public List<GameObject> Units = new List<GameObject>();


    private void Awake()
    {
        // Initialize Units from TempUnitParent
        foreach (Transform child in TempUnitParent.transform)
        {
            Units.Add(child.gameObject);
        }
    }

    private void Update()
    {
        switch (CurrentOrder)
        {
            case Orders.Hold:
                HoldPosition();
                break;
            case Orders.MoveInFormationToTarget:
                MoveInFormationToTarget(TempTarget.transform.position);
                break;
            case Orders.FollowInFormation:
                FollowInFormation(Player.transform.position);
                break;
            case Orders.Charge:
                ChargeAtTarget(TempTarget);
                break;
            case Orders.MoveToTarget:
                GiveOrder_Movement(TempTarget.transform.position);
                break;
            case Orders.AttackAtWill:
                AttackAtWill();
                break;
            case Orders.GuardArea:
                GuardArea(TempTarget.transform.position, 10f);
                break;
        }
    }

    public Vector3 GetSquadCenter()
    {
        Vector3 sum = Vector3.zero;

        foreach (var u in Units)
            sum += u.transform.position;

        return sum / Units.Count;
    }

    private Quaternion GetFormationRotation()
    {
        return Quaternion.LookRotation(Player.transform.forward, Vector3.up);
    }

    private void GiveOrder_Movement(Vector3 target)
    {
        foreach (var u in Units)
            u.GetComponent<UnitAI>().GoTo(target);
    }

    private void HoldPosition()
    {
        Vector3 center = GetSquadCenter();
        Quaternion rot = GetFormationRotation();

        Vector3[] slots = GetSlots(center, rot);
        var assigned = AssignNearestSlots(slots);

        foreach (var u in Units)
        {
            Vector3 slot = assigned[u];
            u.GetComponent<UnitAI>().HoldAtPosition(slot);
        }
    }



    private void MoveInFormationToTarget(Vector3 point)
    {
        Quaternion rot = GetFormationRotation();
        Vector3[] slots = GetSlots(point, rot);

        var assigned = AssignNearestSlots(slots);

        foreach (var u in Units)
        {
            Vector3 slot = assigned[u];
            u.GetComponent<UnitAI>().GoTo(slot);
        }
    }



    private void FollowInFormation(Vector3 playerPos)
    {
        Quaternion rot = GetFormationRotation();
        Vector3[] slots = GetSlots(playerPos, rot);

        var assigned = AssignNearestSlots(slots);

        foreach (var u in Units)
        {
            Vector3 slot = assigned[u];
            u.GetComponent<UnitAI>().FollowFormationSlot(slot);
        }
    }



    private void ChargeAtTarget(GameObject target)
    {
        foreach (var u in Units)
            u.GetComponent<UnitAI>().Attack(target);
    }

    private void AttackAtWill() { }
    private void GuardArea(Vector3 c, float r) { }

    private Vector3[] GetSlots(Vector3 centerPos, Quaternion rot)
    {
        switch (CurrentFormation)
        {
            case FormationTypes.Line:
                return FormationGenerator.GetLineFormation(
                    Units.Count,
                    LineWidth,
                    centerPos,
                    rot,
                    IsLooseFormation
                );

            case FormationTypes.Column:
                return FormationGenerator.GetColumnFormation(
                    Units.Count,
                    ColumnWidth,
                    centerPos,
                    IsLooseFormation,
                    rot
                );

            case FormationTypes.square:
                return FormationGenerator.GetSquareFormation(
                    Units.Count,
                    centerPos,
                    IsLooseFormation,
                    rot
                );
        }

        return null;
    }


    private Dictionary<GameObject, Vector3> AssignNearestSlots(Vector3[] slots)
    {
        Dictionary<GameObject, Vector3> result = new Dictionary<GameObject, Vector3>();

        // Create a list we can remove from
        List<Vector3> remainingSlots = new List<Vector3>(slots);

        foreach (var unit in Units)
        {
            Vector3 bestSlot = remainingSlots[0];
            float bestDist = Vector3.Distance(unit.transform.position, bestSlot);

            // Find nearest
            for (int i = 1; i < remainingSlots.Count; i++)
            {
                float dist = Vector3.Distance(unit.transform.position, remainingSlots[i]);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    bestSlot = remainingSlots[i];
                }
            }

            result[unit] = bestSlot;
            remainingSlots.Remove(bestSlot); // remove so no one else uses it
        }

        return result;
    }


}
