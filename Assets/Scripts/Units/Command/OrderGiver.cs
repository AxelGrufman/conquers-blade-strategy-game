using System.Collections.Generic;
using UnityEngine;

public class OrderGiver : MonoBehaviour
{
    public enum Orders { Hold, MoveInFormationToTarget, FollowInFormation, Charge, MoveToTarget, AttackAtWill, GuardArea, ReturnToMe, none }
    public enum FormationTypes { Line, Column, square }

    public bool IsLooseFormation = false;
    private bool lastIsLooseFormation;
    public Orders CurrentOrder;
    private Orders lastOrder;
    public FormationTypes CurrentFormation;
    private FormationTypes lastFormation;

    public GameObject TempTarget;
    public GameObject Player;

    public int LineWidth = 10;
    public int ColumnWidth = 3;

    public GameObject TempUnitParent;

    public List<GameObject> Units = new List<GameObject>();



    private void Start()
    {
        foreach (Transform child in TempUnitParent.transform)
        {
            Units.Add(child.gameObject);
        }

        lastOrder = CurrentOrder;
        lastFormation = CurrentFormation;
        lastIsLooseFormation = IsLooseFormation;

  
        ApplyOrder(CurrentOrder, true, true);
    }
    private void Update()
    {
        bool orderChanged = CurrentOrder != lastOrder;
        bool formationChanged = CurrentFormation != lastFormation ||
                                IsLooseFormation != lastIsLooseFormation;

        if (orderChanged || formationChanged)
        {
            ApplyOrder(CurrentOrder, orderChanged, formationChanged);

            lastOrder = CurrentOrder;
            lastFormation = CurrentFormation;
            lastIsLooseFormation = IsLooseFormation;
        }

        if (CurrentOrder == Orders.FollowInFormation)
        {
            FollowInFormation(Player.transform.position);
        }
    }

    private void ApplyOrder(Orders order, bool orderChanged, bool formationChanged)
    {
        switch (order)
        {
            case Orders.Hold:
                if (orderChanged || formationChanged)
                    HoldPosition();
                break;

            case Orders.MoveInFormationToTarget:
                if (orderChanged || formationChanged)
                    MoveInFormationToTarget(TempTarget.transform.position);
                break;

            case Orders.FollowInFormation:
                if (formationChanged)
                    FollowInFormation(Player.transform.position);
                break;

            case Orders.ReturnToMe:
                if (orderChanged || formationChanged)
                    ReturnToMe();
                break;

            case Orders.MoveToTarget:
                if (orderChanged)
                    GiveOrder_Movement(TempTarget.transform.position);
                break;

            case Orders.Charge:
                if (orderChanged)
                    ChargeAtTarget(TempTarget);
                break;

            case Orders.AttackAtWill:
                if (orderChanged)
                    AttackAtWill();
                break;

            case Orders.GuardArea:
                if (orderChanged)
                    GuardArea(TempTarget.transform.position, 10f);
                break;
        }
    }



    public Vector3 GetSquadCenter()
    {
        if (Units.Count == 0)
            return transform.position;

        Vector3 sum = Vector3.zero;
        int aliveCount = 0;
        foreach (var u in Units)
        {
            if (u == null) continue;
            sum += u.transform.position;
            aliveCount++;
        }

        return aliveCount > 0 ? sum / aliveCount : transform.position;
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

    private void AttackAtWill()
    {
        foreach (var u in Units)
        {
            var ai = u.GetComponent<UnitAI>();
            ai.EnableAttackAtWill();
        }
    }

    private void GuardArea(Vector3 center, float radius)
    {
        foreach (var u in Units)
        {
            var ai = u.GetComponent<UnitAI>();
            ai.SetGuardArea(center, radius);
        }
    }


    private void ReturnToMe()
    {
        Vector3 center = Player.transform.position;
        Quaternion rot = GetFormationRotation();
        Vector3[] slots = GetSlots(center, rot);
        var assigned = AssignNearestSlots(slots);

        foreach (var u in Units)
        {
            Vector3 slot = assigned[u];
            u.GetComponent<UnitAI>().ReturnToFormation(slot);
        }
    }


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

        List<Vector3> remainingSlots = new List<Vector3>(slots);

        foreach (var unit in Units)
        {
            Vector3 bestSlot = remainingSlots[0];
            float bestDist = Vector3.Distance(unit.transform.position, bestSlot);

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
            remainingSlots.Remove(bestSlot);
        }

        return result;
    }


}
