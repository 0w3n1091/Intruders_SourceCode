using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackPointsAnimationParameterPair
{
    public AttackPoints attackPoints;
    public string animationParameter;
}
public class AttackPointsProvider : MonoBehaviour
{

    public List<AttackPointsAnimationParameterPair> attackPoints;

    public AttackPoints GetAttackPoints(string animationParameter)
    {
        if (animationParameter == null)
            return attackPoints[0].attackPoints;

        foreach (var pair in attackPoints)
        {
            if (pair.animationParameter == animationParameter)
                return pair.attackPoints;
        }

        return attackPoints[0].attackPoints;
    }

}