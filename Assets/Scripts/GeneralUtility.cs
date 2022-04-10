using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralUtility
{
    #region Public Methods
    /// <summary>
    /// Returns the clockwise angle from the global up direction to the given direction.
    /// </summary>
    /// <param name="direction">The direction from one vector position to another to convert.</param>
    public static float ConvertDirectionToAngle(Vector2 direction)
    {
        float angle = -Vector2.SignedAngle(Vector2.up, direction);
        return angle < 0 ? angle + 360f : angle;
    }

    /// <summary>
    /// Returns the normalized vector that is parted from the global up direction by the given angle.
    /// </summary>
    /// <param name="angle">The clockwise angle from the global up direction that defines the direction vector.</param>
    public static Vector2 ConvertAngleToDirection(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Sin(angleRad), Mathf.Cos(angleRad));
    }

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

    public static void CreateWorldTextObject(string name, Vector3 position, Transform parent, string text)
    {
        // create text object in world space
        GameObject gameObject = new GameObject(name, typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.position = position;
        transform.localScale *= 0.1f;
        transform.SetParent(parent);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.fontSize = 20;
        textMesh.color = Color.white;
        textMesh.text = text;
        textMesh.GetComponent<MeshRenderer>().sortingLayerName = "Knight";
    }
    #endregion

    #region Private Methods
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
    #endregion
}
