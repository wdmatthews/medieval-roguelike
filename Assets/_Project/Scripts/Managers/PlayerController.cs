using UnityEngine;
using UnityEngine.InputSystem;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Managers
{
    [AddComponentMenu("Medieval Roguelike/Managers/Player Controller")]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerManagerSO _playerManager;

        private PlayerControllerData _data;
        private int _selectedPrefabIndex;
        private int _prefabCount;

        public PlayerControllerData Data
        {
            get => _data;
            set
            {
                _data = value;
                if (value.SelectedPlayerPrefab) _selectedPrefabIndex = value.SelectedPlayerPrefabIndex;
            }
        }

        private void Awake()
        {
            _prefabCount = _playerManager.PlayerPrefabs.Length;
        }

        public void SelectPreviousCharacter(InputAction.CallbackContext context)
        {
            if (!context.performed || Data.SelectionWasConfirmed) return;
            int index = _selectedPrefabIndex - 1;
            if (index < 0) index = _prefabCount - 1;
            SelectPrefab(index);
        }

        public void SelectNextCharacter(InputAction.CallbackContext context)
        {
            if (!context.performed || Data.SelectionWasConfirmed) return;
            int index = _selectedPrefabIndex + 1;
            if (index >= _prefabCount) index = 0;
            SelectPrefab(index);
        }

        public void ConfirmSelection(InputAction.CallbackContext context)
        {
            if (!context.performed || Data.SelectionWasConfirmed
                || _playerManager.PrefabWasAlreadyChosen(Data.SelectedPlayerPrefab)) return;
            Data.ConfirmSelection();
        }

        public void CancelSelection(InputAction.CallbackContext context)
        {
            if (!context.performed || !Data.SelectionWasConfirmed) return;
            Data.CancelSelection();
        }

        public void SelectPrefab(int index)
        {
            _selectedPrefabIndex = index;
            Player prefab = _playerManager.PlayerPrefabs[_selectedPrefabIndex];
            Data.SelectPlayerPrefab(prefab, _selectedPrefabIndex);
            _animator.runtimeAnimatorController = prefab.GetComponent<Animator>().runtimeAnimatorController;
        }
    }
}
