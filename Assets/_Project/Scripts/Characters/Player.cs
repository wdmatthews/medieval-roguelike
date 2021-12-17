using UnityEngine;
using UnityEngine.InputSystem;
using MedievalRoguelike.UI;

namespace MedievalRoguelike.Characters
{
    [AddComponentMenu("Medieval Roguelike/Characters/Player")]
    public class Player : Character
    {
        [SerializeField] private PlayerInput _input;

        private PlayerHUD _hud;
        private InputAction _moveAction;

        public PlayerInput Input => _input;

        public override void EndDodge()
        {
            base.EndDodge();
            Move(_moveAction.ReadValue<float>());
        }

        public void Spawn(Transform spawnPoint, System.Action<Character> onDeath, PlayerHUD hud)
        {
            Spawn(spawnPoint, onDeath);
            _hud = hud;
            _moveAction = _input.actions.FindAction("Move");
        }

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

        public override bool TakeDamage(float amount)
        {
            bool died = base.TakeDamage(amount);
            _hud.UpdateHealth(_health / _data.MaxHealth);
            return died;
        }
    }
}
