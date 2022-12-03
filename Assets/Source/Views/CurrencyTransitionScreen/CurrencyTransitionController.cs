using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CurrencyTransitionController : ControllerBaseModel
{
    public static CurrencyTransitionController Instance;
    [SerializeField] private PoolModel imagePool;
    public Action onParticleCollected;
    private float playDuration = 5f;
    private float emissionsPerSecond = 10f;
    public Vector2 EmissionDirection = new Vector2(0, 1f);
    public float EmissionAngle = 360f; // Random Range where particles are emitted

    [Header("Spawn and move variables")]
    public Transform TargetTransform;
    public float TargetBumpAmount = .2f;
    public float EndScale = .8f;
    public float MaxScale = 1.6f;
    public float MinRadius = 15f;
    public float MaxRadius = 150f;
    public Ease SpawnEase = Ease.OutQuint;
    public float SpawnTime = 1f;
    public float WaitTime = .3f;
    public Ease MoveEase = Ease.InQuad;
    public float MoveTime = 1f;
    public float ScaleUpDelay = .2f;
    public float ScaleUpTime = .7f;
    public float ScaleDownDelay = .4f;
    public float ScaleDownTime = .6f;

    public override void Initialize()
    {
        base.Initialize();

        if (Instance != null)
            Destroy(Instance);
        else
            Instance = this;

        onParticleCollected += ScreenController.GetScreen<GameScreen>().UpdateMoneyBar;
    }

    public void Emit(Vector3 spawnPos)
    {
        StartCoroutine(emitRoutine(TargetTransform, spawnPos));
    }

    private IEnumerator emitRoutine(Transform target, Vector3 spawnPos)
    {
        float playtime = 0f;
        float emissionPerSecond = emissionsPerSecond;
        float duration = playDuration;
        var particleTimer = 0f;
        Vector3 targetScale = TargetTransform.localScale;

        while (playtime < duration)
        {
            playtime += Time.deltaTime;
            particleTimer += Time.deltaTime;

            while (particleTimer > 1f / emissionPerSecond)
            {
                particleTimer -= 1f / emissionPerSecond;
                CurrencyEffectModel currency = imagePool.GetDeactiveItem<CurrencyEffectModel>();
                setParticleSequence(currency, target, spawnPos);
            }
            yield return new WaitForEndOfFrame();
        }
        DOVirtual.DelayedCall(SpawnTime + WaitTime + MoveTime + .21f, () => TargetTransform.DOScale(targetScale, .1f));
    }

    private void setParticleSequence(CurrencyEffectModel particle, Transform target, Vector3 spawnPos)
    {
        Vector3 randomDirection = Quaternion.AngleAxis(Random.Range(-EmissionAngle / 2f, EmissionAngle / 2f), Vector3.forward) * EmissionDirection;
        randomDirection = randomDirection.normalized * Random.Range(MinRadius, MaxRadius);

        particle.OnSpawn(spawnPos + randomDirection * .3f, .1f * Vector3.one);

        Sequence particleSequence = DOTween.Sequence();

        particleSequence.Insert(0, particle.transform.DOMove(spawnPos + randomDirection, SpawnTime).SetEase(SpawnEase))
            .Insert(SpawnTime + WaitTime, particle.transform.DOMove(target.position, MoveTime).SetEase(MoveEase))
            .Insert(ScaleUpDelay, particle.transform.DOScale(MaxScale, ScaleUpTime))
            .Insert(SpawnTime + WaitTime + ScaleDownDelay, particle.transform.DOScale(Vector3.one * EndScale, ScaleDownTime));

        DOVirtual.DelayedCall(particleSequence.Duration(), () => particle.OnDeactive());
        DOVirtual.DelayedCall(particleSequence.Duration(), () => TargetTransform.DOPunchScale(Vector3.one * Mathf.Clamp(TargetBumpAmount, 0, 1 + TargetBumpAmount + .1f - TargetTransform.localScale.x), .2f, 1, 0));
        DOVirtual.DelayedCall(particleSequence.Duration(), () => onParticleCollected?.Invoke());
    }

    public void EmitParticlesInTime(int count, float t, Vector3 spawnPos)
    {
        emissionsPerSecond = count / t;
        playDuration = t;
        Emit(spawnPos);
    }

    public void EmitParticlesInTime(int count, float t, Transform spawnTransform)
    {
        EmitParticlesInTime(count, t, spawnTransform.position);
    }

    #region Editor Test Variables & Functions
#if UNITY_EDITOR
    [Header("Test Variables")]
    public bool SpawnOnTransform = true;
    public Vector3 TestSpawnPos;
    public Transform TestSpawnTransform;

    public int ParticleCount = 10;
    public float EmitTime = 1f;

    public void E_TestEmit()
    {
        if (SpawnOnTransform)
        {
            EmitParticlesInTime(ParticleCount, EmitTime, TestSpawnTransform);
        }
        else
        {
            if (TestSpawnTransform == null)
            {
                TestSpawnTransform = transform;
            }
            EmitParticlesInTime(ParticleCount, EmitTime, TestSpawnPos);
        }
    }
#endif
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(CurrencyTransitionController))]
public class CurrencyTransitionEditor : Editor
{
    private CurrencyTransitionController uiEditorTarget;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        uiEditorTarget = target as CurrencyTransitionController;
        if (GUILayout.Button("Start Animation"))
        {
            uiEditorTarget.E_TestEmit();
        }
    }
}
#endif