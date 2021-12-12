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

        public void UseDodge(InputAction.CallbackContext context)
        {
            if (context.performed) UseAbility(AbilityType.Dodge);
        }
    }
}
