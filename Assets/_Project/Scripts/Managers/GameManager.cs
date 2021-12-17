using UnityEngine;

namespace MedievalRoguelike.Managers
{
    [AddComponentMenu("Medieval Roguelike/Managers/Game Manager")]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerManagerSO _playerManager;
        [SerializeField] private GameManagerSO _gameManager;

        private void Start()
        {
            _playerManager.SpawnPlayers();
            _gameManager.StartGame();
        }
    }
}
