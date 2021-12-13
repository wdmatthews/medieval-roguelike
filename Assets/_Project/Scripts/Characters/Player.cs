using UnityEngine;
using UnityEngine.InputSystem;

namespace MedievalRoguelike.Characters
{
    [AddComponentMenu("Medieval Roguelike/Characters/Player")]
    public class Player : Character
    {
        public void Move(InputAction.CallbackContext context)
        {
            Move(context.ReadValue<float>());
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed) Jump();
        }

        public void UseAttack1(InputAction.CallbackContext context)
        {
            if (context.performed) UseAbility(AbilityType.Attack1);
        }

        public void UseAttack2(InputAction.CallbackContext context)
        {
            if (context.performed) UseAbility(AbilityType.Attack2);
        }

        public void UseAttack3(InputAction.CallbackContext context)
        {
            if (context.performed) UseAbility(AbilityType.Attack3);
        }

        public void UseDodge(InputAction.CallbackContext context)
        {
            if (context.performed) UseAbility(AbilityType.Dodge);
        }

        public void UseBlock(InputAction.CallbackContext context)
        {
            if (context.performed) UseAbility(AbilityType.Block);
            else if (context.canceled) CancelBlock();
        }
    }
}
