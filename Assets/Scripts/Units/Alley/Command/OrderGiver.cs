using System.Collections.Generic;
using UnityEngine;

public class OrderGiver : MonoBehaviour
{
    public enum Orders { Hold, MoveInFormationToTarget, FollowInFormation, none }
    public enum FormationTypes { Line, Column, square }

    public Orders CurrentOrder;
    public FormationTypes CurrentFormation;

    public Vector3 TargetLocation;
    public GameObject Player;
    public FormationAnchor formationAnchor;

    public int LineWidth = 10;
    public int ColumnWidth = 3;

    public GameObject TempUnitParent;

    [SerializeField] private Camera aimCamera;                 
    [SerializeField] private float aimDistance = 200f;



    public List<GameObject> Units = new List<GameObject>();
    public Dictionary<GameObject, int> UnitSlotIndex = new Dictionary<GameObject, int>();



    private void Start()
    {
        foreach (Transform child in TempUnitParent.transform)
        {
            Units.Add(child.gameObject);
        }

        AssignFixedSlotIndices();
        aimCamera = Camera.main;

    }
    private void Update()
    {
        Vector3[] slot = GetInduviudalPlament();
        switch (CurrentOrder)
        {
            case Orders.Hold:
                    HoldOrder();
                break;

            case Orders.MoveInFormationToTarget:
                    MoveInFormationToTargetOrder(slot);
                break;

            case Orders.FollowInFormation:
               FollowInFormationOrder(slot);  
                break;
        }
    }


    private void AssignFixedSlotIndices()
    {
        UnitSlotIndex.Clear();
        int index = 0;
        foreach (var unit in Units)
        {
            if (unit == null) continue;
            UnitSlotIndex.Add(unit, index);
        }
    }

    private Vector3[] GetInduviudalPlament()
    {
        Vector3[] slots = new Vector3[Units.Count];
        switch (CurrentFormation)
        {
            case FormationTypes.Line:
                slots = FormationGenerator.GetLineFormation(
                    Units.Count, LineWidth,
                    formationAnchor.Position,
                    formationAnchor.Rotaiton);
                break;
            case FormationTypes.Column:
                slots = FormationGenerator.GetColumnFormation(
                    Units.Count, ColumnWidth,
                    formationAnchor.Position,
                    formationAnchor.Rotaiton);
                break;
            case FormationTypes.square:
                slots = FormationGenerator.GetSquareFormation(
                    Units.Count,
                    formationAnchor.Position,
                    formationAnchor.Rotaiton);
                break;
        }
        return slots;
    }


    private void HoldOrder()
    {
        formationAnchor.Stop();
    }
    private void MoveInFormationToTargetOrder(Vector3[] slot)
    {

        formationAnchor.MoveTo(TargetLocation, slot);
    }
    private void FollowInFormationOrder(Vector3[] slot)
    {
        formationAnchor.MoveTo(Player.transform.position, slot);
    }

    public void GetOrder(int index, bool IsOrderMenu)
        {
      if(IsOrderMenu)
            {
            switch (index)
            {
                case 0:
                    TargetLocation = TryGetLookPoint();
                    CurrentOrder = Orders.MoveInFormationToTarget;
                    break;
                case 1:
                    CurrentOrder = Orders.Hold;
                    break;
                case 2:
                    CurrentOrder = Orders.FollowInFormation;
                    break;
            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    CurrentFormation = FormationTypes.Line;
                    break;
                case 1:
                    CurrentFormation = FormationTypes.Column;
                    break;
                case 2:
                    CurrentFormation = FormationTypes.square;
                    break;
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (Units == null || Units.Count == 0)
            return;
        Vector3 center = formationAnchor.Position;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(center, 0.5f);
        Quaternion rot = formationAnchor.Rotaiton;
        Vector3[] slots = GetInduviudalPlament();
        if (slots == null || slots.Length == 0)
            return;
        for (int i = 0; i < slots.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(slots[i], 0.3f);
        }
    }
    private Vector3 TryGetLookPoint()
    {
        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Debug.DrawRay(ray.origin, ray.direction * aimDistance, Color.red, 0.05f);

        if (Physics.Raycast(ray, out RaycastHit hit, aimDistance))
            return hit.point;
        return ray.origin + ray.direction * aimDistance;
    }

}