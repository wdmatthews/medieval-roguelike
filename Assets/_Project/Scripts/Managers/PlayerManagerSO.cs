using System.Collections.Generic;
using UnityEngine;
using MedievalRoguelike.Characters;
using MedievalRoguelike.UI;

namespace MedievalRoguelike.Managers
{
    [CreateAssetMenu(fileName = "Player Manager", menuName = "Medieval Roguelike/Managers/Player Manager")]
    public class PlayerManagerSO : ScriptableObject
    {
        [SerializeField] private Player[] _playerPrefabs;
        [SerializeField] private GameHUDSO _gameHUDReference;
        [SerializeField] private PlayerListSO _alivePlayers;

        [System.NonSerialized] private Player[] _players;
        [System.NonSerialized] private int _alivePlayerCount;
        [System.NonSerialized] private List<PlayerControllerData> _playerControllers;

        public Player[] PlayerPrefabs => _playerPrefabs;

        public List<PlayerControllerData> PlayerControllers
        {
            get => _playerControllers;
            set => _playerControllers = value;
        }

        public System.Action EndGame { get; set; }

        public bool PrefabWasAlreadyChosen(Player prefab)
        {
            foreach (PlayerControllerData controller in PlayerControllers)
            {
                if (controller.SelectionWasConfirmed
                    && controller.SelectedPlayerPrefab == prefab) return true;
            }

            return false;
        }

        public void SpawnPlayers()
        {
            int playerCount = PlayerControllers.Count;
            _players = new Player[playerCount];
            _alivePlayers.Players = new List<Player>();
            _alivePlayerCount = playerCount;
            GameHUD gameHUD = _gameHUDReference.HUD;

            for (int i = 0; i < playerCount; i++)
            {
                PlayerControllerData playerController = PlayerControllers[i];
                Player prefab = playerController.SelectedPlayerPrefab;
                Player player = Instantiate(prefab);
                playerController.Pair(player.Input);
                player.Spawn(null, OnPlayerDeath, gameHUD.AddPlayerHUD(prefab.name, 1));
                _players[i] = player;
                _alivePlayers.AddPlayer(player);
            }
        }

        public void TurnOffPlayers()
        {
            foreach (Player player in _players)
            {
                player.gameObject.SetActive(false);
            }
        }

        public (int, int) GetPointsAndKills()
        {
            int totalPoints = 0;
            int totalKills = 0;

            foreach (Player player in _players)
            {
                totalPoints += player.Points;
                totalKills += player.Kills;
            }

            return (totalPoints, totalKills);
        }

        private void OnPlayerDeath(Character player)
        {
            _alivePlayerCount--;
            _alivePlayers.RemovePlayer((Player)player);
            if (_alivePlayerCount == 0) EndGame?.Invoke();
        }
    }
}
