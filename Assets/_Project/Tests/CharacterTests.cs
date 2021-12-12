using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Tests
{
    public class CharacterTests
    {
        public class Start
        {
            [UnityTest]
            public IEnumerator SetsGravityScale([Values(1, 2)] float gravityScale)
            {
                Character character = A.Character.WithData(A.CharacterSO.WithGravity(gravityScale));

                yield return null;

                Assert.IsTrue(Mathf.Approximately(gravityScale, character.GetComponent<Rigidbody2D>().gravityScale));
            }
        }

        public class Spawn
        {
            [Test]
            public void SetsPosition()
            {
                Character character = A.Character;
                Transform transform = new GameObject().transform;
                transform.position = new Vector3(1, 1, 1);

                character.Spawn(transform, null);

                Assert.IsTrue(Mathf.Approximately(transform.position.x, character.transform.position.x));
                Assert.IsTrue(Mathf.Approximately(transform.position.y, character.transform.position.y));
                Assert.IsTrue(Mathf.Approximately(transform.position.z, character.transform.position.z));
            }

            [Test]
            public void SetsRotation()
            {
                Character character = A.Character;
                Transform transform = new GameObject().transform;
                transform.eulerAngles = new Vector3(1, 1, 1);

                character.Spawn(transform, null);

                Assert.IsTrue(Mathf.Approximately(transform.eulerAngles.x, character.transform.eulerAngles.x));
                Assert.IsTrue(Mathf.Approximately(transform.eulerAngles.y, character.transform.eulerAngles.y));
                Assert.IsTrue(Mathf.Approximately(transform.eulerAngles.z, character.transform.eulerAngles.z));
            }

            [Test]
            public void SetsHealthToMax()
            {
                Character character = A.Character;

                character.Spawn(new GameObject().transform, null);

                Assert.IsTrue(Mathf.Approximately(character.Data.MaxHealth, character.Health));
            }

            [Test]
            public void EnablesHitbox()
            {
                Character character = A.Character;
                BoxCollider2D hitbox = character.GetComponent<BoxCollider2D>();
                hitbox.enabled = false;

                character.Spawn(new GameObject().transform, null);

                Assert.AreEqual(true, hitbox.enabled);
            }
        }

        public class Move
        {
            [Test]
            public void XVelocityIsZeroAndDirectionIsZero_SetsXVelocityToZero()
            {
                Character character = A.Character;
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(0, 0);

                character.Move(0);

                Assert.IsTrue(Mathf.Approximately(0, rigidbody.velocity.x));
            }

            [Test]
            public void XVelocityIsNotZeroAndDirectionIsZero_SetsXVelocityToZero()
            {
                Character character = A.Character;
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(1, 0);

                character.Move(0);

                Assert.IsTrue(Mathf.Approximately(0, rigidbody.velocity.x));
            }

            [Test]
            public void DirectionIsPositive_SetsXVelocityToPositiveMoveSpeed()
            {
                Character character = A.Character;
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();

                character.Move(1);

                Assert.IsTrue(Mathf.Approximately(character.Data.MoveSpeed, rigidbody.velocity.x));
            }

            [Test]
            public void DirectionIsNegative_SetsXVelocityToNegativeMoveSpeed()
            {
                Character character = A.Character;
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();

                character.Move(-1);

                Assert.IsTrue(Mathf.Approximately(-character.Data.MoveSpeed, rigidbody.velocity.x));
            }

            [Test]
            public void DirectionIsZero_DoesNotChangeYAngle([Values(0, 180)] float yAngle)
            {
                Character character = A.Character;
                character.transform.eulerAngles = new Vector3(0, yAngle, 0);

                character.Move(0);

                Assert.IsTrue(Mathf.Approximately(yAngle, character.transform.eulerAngles.y));
            }

            [Test]
            public void FacingRightAndDirectionIsPositive_DoesNotChangeYAngle()
            {
                Character character = A.Character;
                character.transform.eulerAngles = new Vector3(0, 0, 0);

                character.Move(1);

                Assert.IsTrue(Mathf.Approximately(0, character.transform.eulerAngles.y));
            }

            [Test]
            public void FacingLeftAndDirectionIsPositive_SetsYAngleTo0()
            {
                Character character = A.Character;
                character.transform.eulerAngles = new Vector3(0, 180, 0);

                character.Move(1);

                Assert.IsTrue(Mathf.Approximately(0, character.transform.eulerAngles.y));
            }

            [Test]
            public void FacingLeftAndDirectionIsNegative_DoesNotChangeYAngle()
            {
                Character character = A.Character;
                character.transform.eulerAngles = new Vector3(0, 180, 0);

                character.Move(-1);

                Assert.IsTrue(Mathf.Approximately(180, character.transform.eulerAngles.y));
            }

            [Test]
            public void FacingRightAndDirectionIsNegative_SetsYAngleTo180()
            {
                Character character = A.Character;
                character.transform.eulerAngles = new Vector3(0, 0, 0);

                character.Move(-1);

                Assert.IsTrue(Mathf.Approximately(180, character.transform.eulerAngles.y));
            }
        }

        public class Jump
        {
            [UnityTest]
            public IEnumerator CannotJumpAndIsGrounded_DoesNotChangeYVelocity()
            {
                Character character = A.Character.WithData(
                    A.Default.CharacterSO
                        .WithCanJump(false)
                        .WithGravity(0)
                );
                GameObject ground = A.Ground;
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(0, 0);
                yield return new WaitForFixedUpdate();

                character.Jump();

                Assert.IsTrue(Mathf.Approximately(0, rigidbody.velocity.y));
            }

            [Test]
            public void CannotJumpAndIsNotGrounded_DoesNotChangeYVelocity([Values(-1, 0, 1)] float yVelocity)
            {
                Character character = A.Character.WithData(
                    A.Default.CharacterSO
                        .WithCanJump(false)
                        .WithGravity(0)
                );
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(0, yVelocity);

                character.Jump();

                Assert.IsTrue(Mathf.Approximately(yVelocity, rigidbody.velocity.y));
            }

            [Test]
            public void CanJumpAndIsNotGrounded_DoesNotChangeYVelocity([Values(-1, 0, 1)] float yVelocity)
            {
                Character character = A.Character.WithData(
                    A.Default.CharacterSO
                        .WithCanJump(true)
                        .WithGravity(0)
                );
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(0, yVelocity);

                character.Jump();

                Assert.IsTrue(Mathf.Approximately(yVelocity, rigidbody.velocity.y));
            }

            [UnityTest]
            public IEnumerator CanJumpAndIsGrounded_SetsYVelocityToPositive()
            {
                Character character = A.Character.WithData(A.Default.CharacterSO.WithCanJump(true));
                GameObject ground = A.Ground;
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(0, 0);
                yield return new WaitForFixedUpdate();

                character.Jump();

                Assert.Greater(rigidbody.velocity.y, 0);
            }
        }

        public class TakeDamage
        {
            [Test]
            public void DamageIsLessThanHealth_DecreasesHealthByDamage([Values(1, 2, 3)] float maxHealth)
            {
                Character character = A.Character.WithData(A.Default.CharacterSO.WithMaxHealth(maxHealth));
                character.Spawn(new GameObject().transform, null);

                character.TakeDamage(maxHealth / 2);

                Assert.IsTrue(Mathf.Approximately(character.Data.MaxHealth - maxHealth / 2, character.Health));
            }

            [Test]
            public void DamageIsHealth_SetsDeadToTrue()
            {
                Character character = A.Character.WithData(A.Default.CharacterSO.WithMaxHealth(1));
                character.Spawn(new GameObject().transform, null);

                character.TakeDamage(1);

                Assert.IsTrue(character.IsDead);
            }

            [Test]
            public void DamageIsHealth_SetsXVelocityToZero()
            {
                Character character = A.Character.WithData(A.Default.CharacterSO.WithMaxHealth(1));
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(1, 0);
                character.Spawn(new GameObject().transform, null);

                character.TakeDamage(1);

                Assert.IsTrue(Mathf.Approximately(0, rigidbody.velocity.x));
            }

            [Test]
            public void DamageIsHealth_DisablesHitbox()
            {
                Character character = A.Character.WithData(A.Default.CharacterSO.WithMaxHealth(1));
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                BoxCollider2D hitbox = character.GetComponent<BoxCollider2D>();
                hitbox.enabled = true;
                character.Spawn(new GameObject().transform, null);

                character.TakeDamage(1);

                Assert.IsFalse(hitbox.enabled);
            }

            [Test]
            public void DamageIsHealth_InvokesOnDeath()
            {
                Character character = A.Character.WithData(A.Default.CharacterSO.WithMaxHealth(1));
                bool invoked = false;
                character.Spawn(new GameObject().transform, () => invoked = true);

                character.TakeDamage(1);

                Assert.IsTrue(invoked);
            }
        }

        public class StartDodge
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
            public IEnumerator FacingRight_SetsXVelocityToPositiveDodgeSpeed()
            {
                DodgeSO dodgeData = A.Default.DodgeSO;
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(dodgeData));
                character.transform.eulerAngles = new Vector3(0, 0, 0);
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                yield return null;

                character.UseAbility(AbilityType.Dodge);

                Assert.IsTrue(Mathf.Approximately(dodgeData.DodgeSpeed, rigidbody.velocity.x));
            }

            [UnityTest]
            public IEnumerator FacingLeft_SetsXVelocityToNegativeDodgeSpeed()
            {
                DodgeSO dodgeData = A.Default.DodgeSO;
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(dodgeData));
                character.transform.eulerAngles = new Vector3(0, 180, 0);
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                yield return null;

                character.UseAbility(AbilityType.Dodge);

                Assert.IsTrue(Mathf.Approximately(-dodgeData.DodgeSpeed, rigidbody.velocity.x));
            }

            [UnityTest]
            public IEnumerator DisablesHitbox()
            {
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(A.Default.DodgeSO));
                BoxCollider2D hitbox = character.GetComponent<BoxCollider2D>();
                yield return null;

                character.UseAbility(AbilityType.Dodge);

                Assert.IsFalse(hitbox.enabled);
            }
        }

        public class EndDodge
        {
            [UnityTest]
            public IEnumerator SetsIsDodgingToFalse()
            {
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(A.Default.DodgeSO));
                yield return null;
                character.UseAbility(AbilityType.Dodge);

                character.EndDodge();

                Assert.IsFalse(character.IsDodging);
            }

            [UnityTest]
            public IEnumerator EnablesHitbox()
            {
                Character character = A.Character.WithData(A.CharacterSO.WithAbilities(A.Default.DodgeSO));
                BoxCollider2D hitbox = character.GetComponent<BoxCollider2D>();
                yield return null;
                character.UseAbility(AbilityType.Dodge);

                character.EndDodge();

                Assert.IsTrue(hitbox.enabled);
            }
        }
    }
}
