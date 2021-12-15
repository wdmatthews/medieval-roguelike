using UnityEngine;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Managers
{
    public class ManagersTest : MonoBehaviour
    {
        [SerializeField] private PlayerManagerSO _playerManager;
        [SerializeField] private GameManagerSO _gameManager;
        [SerializeField] private Player[] _playerPrefabs;

        private void Start()
        {
            _playerManager.SpawnPlayers(_playerPrefabs);
            _gameManager.StartGame();
        }
    }
}
