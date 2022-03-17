using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralUtility
{
    public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    public static void SwapAnimatorController(Animator animator, RuntimeAnimatorController to, bool maintainState)
    {
        if (!maintainState)
        {
            animator.runtimeAnimatorController = to;
            return;
        }

        // save animator state
        AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[animator.layerCount];
        for (int i = 0; i < layerInfo.Length; i++)
        {
            layerInfo[i] = animator.GetCurrentAnimatorStateInfo(i);
        }

        // save parameter values
        Dictionary<string, bool> boolParameters = new Dictionary<string, bool>();
        Dictionary<string, float> floatParameters = new Dictionary<string, float>();
        Dictionary<string, int> intParameters = new Dictionary<string, int>();
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Bool:
                    boolParameters.Add(parameter.name, animator.GetBool(parameter.name));
                    break;
                case AnimatorControllerParameterType.Float:
                    floatParameters.Add(parameter.name, animator.GetFloat(parameter.name));
                    break;
                case AnimatorControllerParameterType.Int:
                    intParameters.Add(parameter.name, animator.GetInteger(parameter.name));
                    break;
            }
        }

        animator.runtimeAnimatorController = to;

        // restore parameter values
        foreach (KeyValuePair<string, bool> parameter in boolParameters)
        {
            animator.SetBool(parameter.Key, parameter.Value);
        }

        foreach (KeyValuePair<string, float> parameter in floatParameters)
        {
            animator.SetFloat(parameter.Key, parameter.Value);
        }

        foreach (KeyValuePair<string, int> parameter in intParameters)
        {
            animator.SetInteger(parameter.Key, parameter.Value);
        }

        // push back saved state
        for (int i = 0; i < layerInfo.Length; i++)
        {
            animator.Play(layerInfo[i].fullPathHash, i, layerInfo[i].normalizedTime);
        }

        animator.Update(0.0f);
    }
}
