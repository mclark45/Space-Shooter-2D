using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public void StartCameraShake()
    {
        StartCoroutine(CameraShake(.2f, .1f));
    }



    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 cameraPosition = transform.localPosition;

        float timePassed = 0.0f;

        while (timePassed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, cameraPosition.z);

            timePassed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = cameraPosition;
    }
}
