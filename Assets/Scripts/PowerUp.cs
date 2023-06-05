using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpMovement movement { get; private set; }
    public Rigidbody2D rigidbody { get; private set; }
    public CapsuleCollider2D collider { get; private set; }
    public bool canCollide { get; private set; }

    private void Awake()
    {
        movement = GetComponent<PowerUpMovement>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }
    private void OnEnable()
    {
        StartExit();
    }
    private void StartExit()
    {
        StartCoroutine(ExitCoroutine());
    }
    private IEnumerator ExitCoroutine()
    {
        this.rigidbody.isKinematic = true;
        canCollide = false;
        Vector2 outsidePosition = new Vector2(this.transform.position.x, this.transform.position.y + 0.8f);
        Vector3 position = this.transform.position;
        float elapsed = 0, duration = 1f;

        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, outsidePosition, elapsed / duration);
            newPosition.z = position.z;
            this.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
        canCollide = true;
        this.rigidbody.isKinematic = false;
    }
}
