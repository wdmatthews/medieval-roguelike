using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Managers
{
    [System.Serializable]
    public class PlayerControllerData
    {
        private InputDevice _device;
        private int _index;
        private Player _selectedPlayerPrefab;
        private int _selectedPlayerPrefabIndex;
        private bool _selectionWasConfirmed;

        public InputDevice Device => _device;
        public Player SelectedPlayerPrefab => _selectedPlayerPrefab;
        public int SelectedPlayerPrefabIndex => _selectedPlayerPrefabIndex;
        public bool SelectionWasConfirmed => _selectionWasConfirmed;
        public System.Action<int> OnConfirm { get; set; }
        public System.Action<int> OnCancel { get; set; }

        public PlayerControllerData(InputDevice device, int index, System.Action<int> onConfirm, System.Action<int> onCancel)
        {
            _device = device;
            _index = index;
            OnConfirm = onConfirm;
            OnCancel = onCancel;
        }

        public void SelectPlayerPrefab(Player prefab, int prefabIndex)
        {
            _selectedPlayerPrefab = prefab;
            _selectedPlayerPrefabIndex = prefabIndex;
        }

        public void ConfirmSelection()
        {
            _selectionWasConfirmed = true;
            OnConfirm?.Invoke(_index);
        }

        public void CancelSelection()
        {
            _selectionWasConfirmed = false;
            OnCancel?.Invoke(_index);
        }

        public void Pair(PlayerInput input)
        {
            InputUser.PerformPairingWithDevice(_device, input.user);
            input.SwitchCurrentControlScheme(_device);
        }
    }
}
