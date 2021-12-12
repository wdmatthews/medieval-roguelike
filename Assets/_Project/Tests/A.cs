using UnityEngine;

namespace MedievalRoguelike.Tests
{
    public static class A
    {
        public static CharacterSOBuilder CharacterSO => new CharacterSOBuilder();
        public static CharacterBuilder Character => new CharacterBuilder();
        public static GroundBuilder Ground => new GroundBuilder();
        public static DodgeSOBuilder DodgeSO => new DodgeSOBuilder();

        public static class Default
        {
            public static CharacterSOBuilder CharacterSO => A.CharacterSO
                .WithMoveSpeed(1)
                .WithJumpHeight(1)
                .WithGravity(1)
                .WithCanJump(true)
                .WithGroundCheckPosition(0)
                .WithGroundCheckSize(new Vector2(1, 1));
            public static DodgeSOBuilder DodgeSO => A.DodgeSO
                .WithDodgeSpeed(1)
                .WithCooldownDuration(1);
        }
    }
}
