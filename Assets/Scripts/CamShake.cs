using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    [SerializeField]
    Camera camToShake;
    bool shaking;

    private void Start()
    {
        ShelfTrigger.HitAnyShelf += ScreenShake;        
    }

    void ScreenShake()
    {
        if (!shaking)
        {
            float d = Random.Range(0.075f, 0.10f);
            float m = Random.Range(0.04f, 0.07f);
            StartCoroutine(Shake(d,m));
        }
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        shaking = true;
        Vector3 originalPos = camToShake.transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            camToShake.transform.localPosition = new Vector3(x + originalPos.x, y + originalPos.y, originalPos.z);

            elapsedTime += Time.deltaTime;
            yield return null;      //waits for the next frame before continuing while loop
        }

        camToShake.transform.localPosition = originalPos;
        shaking = false;
    }
}
