using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Tests
{
    public class DodgeTests
    {
        public class Use
        {
            [UnityTest]
            public IEnumerator SetsIsDodgingToTrue()
            {
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(A.Default.DodgeSO));
                yield return null;

                character.UseAbility(AbilityType.Dodge);

                Assert.IsTrue(character.IsDodging);
            }

            [UnityTest]
            public IEnumerator CanUseIsFalse()
            {
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(A.Default.DodgeSO));
                yield return null;
                Dodge dodge = (Dodge)character.AbilitiesByType[AbilityType.Dodge];

                character.UseAbility(AbilityType.Dodge);

                Assert.IsFalse(dodge.CanUse);
            }
        }

        public class OnUpdate
        {
            [UnityTest]
            public IEnumerator CooldownPasses_CanUseIsTrue()
            {
                DodgeSO dodgeData = A.Default.DodgeSO;
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(dodgeData));
                yield return null;
                Dodge dodge = (Dodge)character.AbilitiesByType[AbilityType.Dodge];
                character.UseAbility(AbilityType.Dodge);
                character.OnAbilityAnimationEnd();

                Time.timeScale = 60;
                yield return new WaitForSeconds(dodgeData.CooldownDuration);
                Time.timeScale = 1;

                Assert.IsTrue(dodge.CanUse);
            }
        }

        public class Reset
        {
            [UnityTest]
            public IEnumerator CanUseIsTrue()
            {
                DodgeSO dodgeData = A.Default.DodgeSO;
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(dodgeData));
                yield return null;
                Dodge dodge = (Dodge)character.AbilitiesByType[AbilityType.Dodge];
                character.UseAbility(AbilityType.Dodge);
                character.OnAbilityAnimationEnd();

                dodge.Reset();

                Assert.IsTrue(dodge.CanUse);
            }
        }
    }
}
