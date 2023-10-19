using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtilsMath
{
    public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
    {
        if (Quaternion.Dot(a, b) < 0)
        {
            return a * Quaternion.Inverse(Multiply(b, -1));
        }
        else return a * Quaternion.Inverse(b);
    }
    
    public static Quaternion Multiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }

    public static List<Transform> OrderTransformListByDistance(List<Transform> unorderedList, Transform positionToCompare)
    {
        List<Transform> orderedList = unorderedList.OrderBy(x => Vector2.Distance(positionToCompare.position,x.transform.position)).ToList();
        return orderedList;
    }
    
    public static List<DragRigidbody> OrderDragRigidbodyListByDistance(List<DragRigidbody> unorderedList, Transform positionToCompare)
    {
        List<DragRigidbody> orderedList = unorderedList.OrderBy(x => Vector2.Distance(positionToCompare.position,x.transform.position)).ToList();
        return orderedList;
    }
    
    public static List<MovableRigidbody> OrderMovableRigidbodiesListByDistance(List<MovableRigidbody> unorderedList, Transform positionToCompare)
    {
        List<MovableRigidbody> orderedList = unorderedList.OrderBy(x => Vector2.Distance(positionToCompare.position,x.transform.position)).ToList();
        return orderedList;
    }
    
}
