using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using MedievalRoguelike.UI;

namespace MedievalRoguelike.Managers
{
    [AddComponentMenu("Medieval Roguelike/Managers/Player Manager")]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerManagerSO _playerManager;
        [SerializeField] private GameManagerSO _gameManager;
        [SerializeField] private PlayerController _controllerPrefab;
        [SerializeField] private CharacterSelection _characterSelectionUI;
        [SerializeField] private Vector2[] _controllerPositions;

        private void Start()
        {
            _characterSelectionUI.OnStartGame = StartGame;
            _gameManager.CurrentRegion = _gameManager.Regions[Random.Range(0, _gameManager.Regions.Length)];

            if (_playerManager.PlayerControllers == null) _playerManager.PlayerControllers = new List<PlayerControllerData>();

            if (_playerManager.PlayerControllers.Count > 0)
            {
                for (int i = 0; i < _playerManager.PlayerControllers.Count; i++)
                {
                    PlayerControllerData controllerData = _playerManager.PlayerControllers[i];
                    PlayerController controller = Instantiate(_controllerPrefab);
                    controllerData.CancelSelection();
                    controllerData.OnConfirm = OnPlayerConfirmed;
                    controllerData.OnCancel = OnPlayerCanceled;
                    JoinPlayer(i, controller, controllerData.Device, controllerData);
                }
            }
            else
            {
                foreach (InputDevice device in InputSystem.devices)
                {
                    if (device is Keyboard || device is Gamepad)
                    {
                        PlayerController controller = Instantiate(_controllerPrefab);
                        JoinPlayer(_playerManager.PlayerControllers.Count, controller, device);
                    }
                }
            }
        }

        public void JoinPlayer(int index, PlayerController controller,
            InputDevice device, PlayerControllerData existingControllerData = null)
        {
            PlayerInput input = controller.GetComponent<PlayerInput>();
            InputUser.PerformPairingWithDevice(device, input.user);
            input.SwitchCurrentControlScheme(device);

            PlayerControllerData controllerData = existingControllerData ?? new PlayerControllerData(device,
                _playerManager.PlayerControllers.Count, OnPlayerConfirmed, OnPlayerCanceled);
            controller.Data = controllerData;

            controller.transform.position = _controllerPositions[index];

            if (existingControllerData == null)
            {
                controller.SelectPrefab(0);
                _playerManager.PlayerControllers.Add(controllerData);
            }
            else controller.SelectPrefab(controllerData.SelectedPlayerPrefabIndex);
        }

        private void OnPlayerConfirmed(int index)
        {
            bool allPlayersConfirmed = true;

            foreach (PlayerControllerData controller in _playerManager.PlayerControllers)
            {
                if (!controller.SelectionWasConfirmed)
                {
                    allPlayersConfirmed = false;
                    break;
                }
            }

            if (allPlayersConfirmed) _characterSelectionUI.ShowStartButton();
        }

        private void OnPlayerCanceled(int index)
        {
            _characterSelectionUI.HideStartButton();
        }

        private void StartGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
