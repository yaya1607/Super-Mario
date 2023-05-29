using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(BounceCoroutine());
    }

    private IEnumerator BounceCoroutine()
    {
        Vector3 bouncedPosition = new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z),
            originalPosition = this.transform.position;

        float duration = 0.2f;
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
        while (elapsed <= duration)
        {
            Vector3 newPosition = Vector3.Lerp( bouncedPosition, originalPosition, elapsed / duration);
            newPosition.z = originalPosition.z;
            this.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Inactive();
    }
    private void Inactive()
    {
        this.gameObject.SetActive(false);
    }
}
