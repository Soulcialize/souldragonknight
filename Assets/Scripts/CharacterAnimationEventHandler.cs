using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent executeAttackEffectEvent;
    [SerializeField] private UnityEvent attackRecoveryEvent;
    [SerializeField] private UnityEvent attackRecoveryEndEvent;
    [SerializeField] private UnityEvent stunEndEvent;
    [SerializeField] private UnityEvent hurtEndEvent;

    public void OnExecuteAttackEffect()
    {
        executeAttackEffectEvent.Invoke();
    }

    public void OnAttackRecovery()
    {
        attackRecoveryEvent.Invoke();
    }

    public void OnAttackRecoveryEnd()
    {
        attackRecoveryEndEvent.Invoke();
    }

    public void OnStunEnd()
    {
        stunEndEvent.Invoke();
    }

    public void OnHurtEnd()
    {
        hurtEndEvent.Invoke();
    }
}
