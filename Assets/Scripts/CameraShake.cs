using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraShake : MonoBehaviour
{

    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private Vector3 originalPos;
    private Quaternion originalRotation;
    private float shakeDuration = 0f;
    void OnEnable()
    {
        originalPos = transform.localPosition;
        originalRotation = transform.rotation;
    }
    public void OnCameraShake(float shakeduration)
    {
        enabled = true;
        shakeDuration = shakeduration;
        StartCoroutine(Coroutine());
    }
    IEnumerator Coroutine()
    {
        while(shakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
            yield return null;
        }
    }

}
