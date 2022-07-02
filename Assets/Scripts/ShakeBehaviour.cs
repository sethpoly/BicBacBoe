using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShakeBehaviour : MonoBehaviour
{ 
    [Header("Settings")]
    [Range(0f, 200f)]
    [SerializeField] public float shake_speed = 1f;
    [SerializeField] public Vector3 originPosition;
    [SerializeField] public bool isShaking = false;

    //
    public float shakeTimer = 0f;
    public float shakeAmount = .2f;
    public float shakeSpeed = 10;
    Vector3 newPos;
    //

    private IEnumerator Shake(Action onCompletion)
    {
        while (shakeTimer > 0)
        {
            isShaking = true;

            if (Vector3.Distance(newPos, transform.position) <= shakeAmount / 30)
            {
                newPos = originPosition + UnityEngine.Random.insideUnitSphere * shakeAmount;
            }

            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * shakeSpeed);
            shakeTimer -= Time.deltaTime;
            yield return null;
        }

        shakeTimer = 0f;
        transform.position = originPosition;
        isShaking = false;
        onCompletion();
    }

    public void ShakeForTime(float timeInSeconds, Action callback)
    {
        if(!isShaking) {
            if(timeInSeconds <= 0) {
                Debug.LogError("Shake time must be > 0");
                 return; 
            }

            originPosition = transform.position;
            newPos = originPosition;
            shakeTimer = timeInSeconds;
            StartCoroutine(Shake(onCompletion: callback));
        }
    }

    public void CancelShake()
    {
        shakeTimer = 0;
        isShaking = false;
        transform.position = originPosition;
    }
}
