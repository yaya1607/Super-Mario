using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default");
    private static int enemy = LayerMask.NameToLayer("Enemy");

    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if (rigidbody.isKinematic) {
            return false;
        }

        float radius = 0.25f;
        float distance = 0.265f;

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);
        return hit.collider != null && hit.rigidbody != rigidbody;
    }
    //This function I added to check the enemies standing on the block. 
    public static List<RaycastHit2D> StandingCheck(this Transform transform, Vector2 direction)
    {
        Vector2 boxSize = new Vector2(1f, 0.5f);
        Vector2 checkPoint = new Vector2(transform.position.x, transform.position.y + 0.5f);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(checkPoint,boxSize,0f,direction,0.5f);
        List<RaycastHit2D> listEnemy = new List<RaycastHit2D>();
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.layer == enemy)
                listEnemy.Add(hit);
        }
        return listEnemy ;
    }
    public static bool UndergroundPipeInCheck(this Rigidbody2D rigidbody)
    {
        float radius = 0.25f;
        float distance = 0.265f;

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, Vector2.down, distance, layerMask);
        return hit.collider.CompareTag("UndergroundPipeIn") && (Mathf.Abs(rigidbody.position.x - hit.collider.transform.position.x) <= 0.35) && hit.rigidbody != rigidbody;
    }
    public static bool UndergroundPipeOutCheck(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return false;
        }
        Vector2 boxSize = new Vector2(0.1f, 0.25f);
        float distance = 0.335f;

        RaycastHit2D hit = Physics2D.BoxCast(rigidbody.position, boxSize, 0f, direction, distance, layerMask);
        if(hit.collider != null)
        {
            return hit.collider.CompareTag("UndergroundPipeOut") && hit.rigidbody != rigidbody && Mathf.Abs(rigidbody.position.x - hit.collider.transform.position.x) < 1f;
        }
        return false;
    }
    //This function has a bit difference from tutorial that it has angleInDegree to make more easy to change the angle of DotTest, just to make it more flexible.
    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection, float angleInDegrees)
    {
        Vector2 direction = other.position - transform.position;
        float angleInRad = Mathf.Cos(angleInDegrees * Mathf.Deg2Rad);
        return Vector2.Dot(direction.normalized, testDirection) >= angleInRad;
    }
    
}
