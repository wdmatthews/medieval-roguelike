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

                Assert.AreEqual(gravityScale, character.GetComponent<Rigidbody2D>().gravityScale);
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

                Assert.AreEqual(0, rigidbody.velocity.x);
            }

            [Test]
            public void XVelocityIsNotZeroAndDirectionIsZero_SetsXVelocityToZero()
            {
                Character character = A.Character;
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(1, 0);

                character.Move(0);

                Assert.AreEqual(0, rigidbody.velocity.x);
            }

            [Test]
            public void DirectionIsPositive_SetsXVelocityToPositiveMoveSpeed()
            {
                Character character = A.Character;
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();

                character.Move(1);

                Assert.AreEqual(character.Data.MoveSpeed, rigidbody.velocity.x);
            }

            [Test]
            public void DirectionIsNegative_SetsXVelocityToNegativeMoveSpeed()
            {
                Character character = A.Character;
                Rigidbody2D rigidbody = character.GetComponent<Rigidbody2D>();

                character.Move(-1);

                Assert.AreEqual(-character.Data.MoveSpeed, rigidbody.velocity.x);
            }

            [Test]
            public void DirectionIsZero_DoesNotChangeYAngle([Values(0, 180)] float yAngle)
            {
                Character character = A.Character;
                character.transform.eulerAngles = new Vector3(0, yAngle, 0);

                character.Move(0);

                Assert.AreEqual(yAngle, character.transform.eulerAngles.y);
            }

            [Test]
            public void FacingRightAndDirectionIsPositive_DoesNotChangeYAngle()
            {
                Character character = A.Character;
                character.transform.eulerAngles = new Vector3(0, 0, 0);

                character.Move(1);

                Assert.AreEqual(0, character.transform.eulerAngles.y);
            }

            [Test]
            public void FacingLeftAndDirectionIsPositive_SetsYAngleTo0()
            {
                Character character = A.Character;
                character.transform.eulerAngles = new Vector3(0, 180, 0);

                character.Move(1);

                Assert.AreEqual(0, character.transform.eulerAngles.y);
            }

            [Test]
            public void FacingLeftAndDirectionIsNegative_DoesNotChangeYAngle()
            {
                Character character = A.Character;
                character.transform.eulerAngles = new Vector3(0, 180, 0);

                character.Move(-1);

                Assert.AreEqual(180, character.transform.eulerAngles.y);
            }

            [Test]
            public void FacingRightAndDirectionIsNegative_SetsYAngleTo180()
            {
                Character character = A.Character;
                character.transform.eulerAngles = new Vector3(0, 0, 0);

                character.Move(-1);

                Assert.AreEqual(180, character.transform.eulerAngles.y);
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

                Assert.AreEqual(0, rigidbody.velocity.y);
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

                Assert.AreEqual(yVelocity, rigidbody.velocity.y);
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

                Assert.AreEqual(yVelocity, rigidbody.velocity.y);
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
    }
}
