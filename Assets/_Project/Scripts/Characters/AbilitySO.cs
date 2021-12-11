using UnityEngine;

namespace MedievalRoguelike.Characters
{
    public abstract class AbilitySO : ScriptableObject
    {
        public AbilityType GetAbilityType()
        {
            if (this is AttackSO) return ((AttackSO)this).Type;
            else if (this is DodgeSO) return AbilityType.Dodge;
            else if (this is BlockSO) return AbilityType.Block;
            return AbilityType.Attack1;
        }
    }
}
