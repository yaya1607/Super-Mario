using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowShroom : MonoBehaviour
{
    public ShroomMovement movement { get; private set; }
    public Rigidbody2D rigidbody { get; private set; }
    private float elapsed;
    
    private void Awake()
    {
        movement = GetComponent<ShroomMovement>();
        rigidbody = GetComponent<Rigidbody2D>();
        StartExit(0f);
    }
    public void StartExit(float elapsed)
    {
        StartCoroutine(ExitCoroutine(elapsed));
    }
    private IEnumerator ExitCoroutine(float elapsed)
    {
        this.rigidbody.isKinematic = true;
        this.movement.enabled = false;
        Vector2 outsidePosition = new Vector2(this.transform.position.x,this.transform.position.y + 0.8f);
        Vector3 position = this.transform.position;
        float duration = 1f;

        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, outsidePosition, elapsed / duration);
            newPosition.z = position.z;
            this.transform.position = newPosition;
            elapsed += Time.deltaTime;
            this.elapsed = elapsed;
            yield return null;
        }

        this.rigidbody.isKinematic = false;
        this.movement.enabled = true;
        this.elapsed = 0;
    }
}
