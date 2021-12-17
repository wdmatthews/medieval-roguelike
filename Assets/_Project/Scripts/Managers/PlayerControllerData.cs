using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Managers
{
    public class PlayerControllerData
    {
        private InputDevice _device;
        private int _index;
        private Player _selectedPlayerPrefab;
        private bool _selectionWasConfirmed;
        private System.Action<int> _onConfirm;
        private System.Action<int> _onCancel;

        public Player SelectedPlayerPrefab => _selectedPlayerPrefab;
        public bool SelectionWasConfirmed => _selectionWasConfirmed;

        public PlayerControllerData(InputDevice device, int index, System.Action<int> onConfirm, System.Action<int> onCancel)
        {
            _device = device;
            _index = index;
            _onConfirm = onConfirm;
            _onCancel = onCancel;
        }

        public void SelectPlayerPrefab(Player prefab)
        {
            _selectedPlayerPrefab = prefab;
        }

        public void ConfirmSelection()
        {
            _selectionWasConfirmed = true;
            _onConfirm?.Invoke(_index);
        }

        public void CancelSelection()
        {
            _selectionWasConfirmed = false;
            _onCancel?.Invoke(_index);
        }

        public void Pair(PlayerInput input)
        {
            InputUser.PerformPairingWithDevice(_device, input.user);
            input.SwitchCurrentControlScheme(_device);
        }
    }
}
