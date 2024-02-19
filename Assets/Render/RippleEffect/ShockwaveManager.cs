using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveManager : MonoBehaviour
{
    [SerializeField] private float _ShockwaveTime = 0.75f;
    private Coroutine _ShockwaveCoroutine;
    private Material _material;
    private static int _waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");
    private static int _shockwaveStrength = Shader.PropertyToID("_ShockwaveStrength");
    private static int _size = Shader.PropertyToID("_Size");

    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }
    private void OnEnable()
    {
        EventManager.StartListening("PD_DashStarted", CallShockwaveDash);
        EventManager.StartListening("TS_StopTime", CallShockwaveTS);
    }
    private void OnDisable()
    {
        EventManager.StopListening("PD_DashStarted", CallShockwaveDash);
        EventManager.StopListening("TS_StopTime", CallShockwaveTS);
    }

    public void CallShockwaveDash()
    {
        _material.SetFloat(_size, 0.05f);
        _material.SetFloat(_shockwaveStrength, 0.075f);
        if (_ShockwaveCoroutine != null)
        {
            StopCoroutine( _ShockwaveCoroutine );
        }
        _ShockwaveCoroutine = StartCoroutine(ShockwaveAction(-0.1f, 1f));
    }
    public void CallShockwaveTS()
    {
        _material.SetFloat(_size, 0.1f);
        _material.SetFloat(_shockwaveStrength, 2f);
        if (_ShockwaveCoroutine != null)
        {
            StopCoroutine(_ShockwaveCoroutine);
        }
        _ShockwaveCoroutine = StartCoroutine(ShockwaveAction(-0.1f, 1f));
    }

    private IEnumerator ShockwaveAction(float StartPos, float EndPos)
    {
        _material.SetFloat(_waveDistanceFromCenter, StartPos);

        float lerpedAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _ShockwaveTime)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(StartPos, EndPos, (elapsedTime / _ShockwaveTime));
            _material.SetFloat(_waveDistanceFromCenter, lerpedAmount);

            yield return null;
        }
    }
}
