using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Tests
{
    public class BlockTests
    {
        public class Use
        {
            [UnityTest]
            public IEnumerator SetsIsBlockingToTrue()
            {
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(A.Default.BlockSO));
                yield return null;

                character.UseAbility(AbilityType.Block);

                Assert.IsTrue(character.IsBlocking);
            }

            [UnityTest]
            public IEnumerator EnoughBlockTimeLeft_CanUseIsTrue()
            {
                BlockSO blockData = A.Default.BlockSO;
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(blockData));
                yield return null;
                Block block = (Block)character.AbilitiesByType[AbilityType.Block];

                character.UseAbility(AbilityType.Block);

                Assert.IsTrue(block.CanUse);
            }

            [UnityTest]
            public IEnumerator NoBlockTimeLeft_CanUseIsFalse()
            {
                BlockSO blockData = A.Default.BlockSO;
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(blockData));
                yield return null;
                Block block = (Block)character.AbilitiesByType[AbilityType.Block];
                character.UseAbility(AbilityType.Block);

                Time.timeScale = 60;
                yield return new WaitForSeconds(blockData.MaxBlockDuration);
                Time.timeScale = 1;

                Assert.IsFalse(block.CanUse);
            }
        }

        public class OnUpdate
        {
            [UnityTest]
            public IEnumerator NoBlockTimeLeftAndCooldownPassesAndRechargesEnough_CanUseIsFalse()
            {
                BlockSO blockData = A.Default.BlockSO;
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(blockData));
                yield return null;
                Block block = (Block)character.AbilitiesByType[AbilityType.Block];
                character.UseAbility(AbilityType.Block);

                Time.timeScale = 60;
                yield return new WaitForSeconds(blockData.MaxBlockDuration + blockData.CooldownDuration);
                Time.timeScale = 1;

                Assert.IsFalse(block.CanUse);
            }
        }

        public class Reset
        {
            [UnityTest]
            public IEnumerator CanUseIsTrue()
            {
                BlockSO blockData = A.Default.BlockSO;
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(blockData));
                yield return null;
                Block block = (Block)character.AbilitiesByType[AbilityType.Block];
                character.UseAbility(AbilityType.Block);
                character.OnAbilityAnimationEnd();

                block.Reset();

                Assert.IsTrue(block.CanUse);
            }
        }
    }
}
