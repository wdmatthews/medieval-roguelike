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

        public Player[] PlayerPrefabs => _playerPrefabs;
        public System.Action EndGame { get; set; }

        public void SpawnPlayers(Player[] prefabs)
        {
            int playerCount = prefabs.Length;
            _players = new Player[playerCount];
            _alivePlayers.Players = new List<Player>();
            _alivePlayerCount = playerCount;
            GameHUD gameHUD = _gameHUDReference.HUD;

            for (int i = 0; i < playerCount; i++)
            {
                Player prefab = prefabs[i];
                Player player = Instantiate(prefab);
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

        private void OnPlayerDeath(Character player)
        {
            _alivePlayerCount--;
            _alivePlayers.RemovePlayer((Player)player);
            if (_alivePlayerCount == 0) EndGame?.Invoke();
        }
    }
}
