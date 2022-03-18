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
        Dictionary<string, (AnimatorControllerParameterType, object)> savedParameters
            = SaveAnimatorParameters(animator);

        animator.runtimeAnimatorController = to;

        // restore parameter values
        RestoreAnimatorParameters(animator, savedParameters);

        // push back saved state
        for (int i = 0; i < layerInfo.Length; i++)
        {
            animator.Play(layerInfo[i].fullPathHash, i, layerInfo[i].normalizedTime);
        }

        animator.Update(0.0f);
    }

    private static Dictionary<string, (AnimatorControllerParameterType, object)> SaveAnimatorParameters(Animator animator)
    {
        Dictionary<string, (AnimatorControllerParameterType, object)> savedParameters
            = new Dictionary<string, (AnimatorControllerParameterType, object)>();

        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Bool:
                    savedParameters[parameter.name] = (parameter.type, animator.GetBool(parameter.name));
                    break;
                case AnimatorControllerParameterType.Float:
                    savedParameters[parameter.name] = (parameter.type, animator.GetFloat(parameter.name));
                    break;
                case AnimatorControllerParameterType.Int:
                    savedParameters[parameter.name] = (parameter.type, animator.GetInteger(parameter.name));
                    break;
            }
        }

        return savedParameters;
    }

    private static void RestoreAnimatorParameters(
        Animator animator, Dictionary<string, (AnimatorControllerParameterType, object)> savedParameters)
    {
        string name;
        AnimatorControllerParameterType type;
        object value;
        foreach (KeyValuePair<string, (AnimatorControllerParameterType, object)> nameValuePair in savedParameters)
        {
            name = nameValuePair.Key;
            type = nameValuePair.Value.Item1;
            value = nameValuePair.Value.Item2;

            switch (type)
            {
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(name, (bool)value);
                    break;
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(name, (float)value);
                    break;
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(name, (int)value);
                    break;
            }
        }
    }
}
