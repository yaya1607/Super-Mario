using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public virtual void SmallHit() 
    {
        //Make a bounce when being hit
        StartCoroutine(BounceCoroutine());
    }
    public virtual void Hit()
    {
        //When big Mario hit the block, if enemies were standing in the block, it will fall out.
        KillEnemiesAreStandingIn();
    }
    
    private IEnumerator BounceCoroutine()
    {
        Vector3 bouncedPosition = new Vector3(this.transform.position.x, this.transform.position.y + 0.25f, this.transform.position.z),
            originalPosition = this.transform.position;

        float duration = 0.1f;
        float elapsed = 0;
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(originalPosition, bouncedPosition, elapsed / duration);
            newPosition.z = originalPosition.z;
            this.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
        elapsed = 0;
        while (elapsed <= duration + 0.1f)
        {
            Vector3 newPosition = Vector3.Lerp(bouncedPosition, originalPosition, elapsed / duration);
            newPosition.z = originalPosition.z;
            this.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    protected void KillEnemiesAreStandingIn()
    {
        List<RaycastHit2D> listEnemy = transform.StandingCheck(Vector2.up);
        foreach (RaycastHit2D hit in listEnemy)
        {
            RaycastHit2D enemy = hit;
            enemy.collider.GetComponent<Movement>().FallOut();
        }
    }
}
