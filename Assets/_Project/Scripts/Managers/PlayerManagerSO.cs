using System.Collections.Generic;
using UnityEngine;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Managers
{
    [CreateAssetMenu(fileName = "Player Manager", menuName = "Medieval Roguelike/Managers/Player Manager")]
    public class PlayerManagerSO : ScriptableObject
    {
        [SerializeField] private Player[] _playerPrefabs;

        [System.NonSerialized] private Player[] _players;
        [System.NonSerialized] private List<Player> _alivePlayers;
        [System.NonSerialized] private int _alivePlayerCount;

        public Player[] PlayerPrefabs => _playerPrefabs;
        public List<Player> AlivePlayers => _alivePlayers;
        public System.Action EndGame { get; set; }

        public void SpawnPlayers(Player[] prefabs)
        {
            int playerCount = prefabs.Length;
            _players = new Player[playerCount];
            _alivePlayers = new List<Player>();
            _alivePlayerCount = playerCount;

            for (int i = 0; i < playerCount; i++)
            {
                Player player = Instantiate(prefabs[i]);
                player.Spawn(null, OnPlayerDeath);
                _players[i] = player;
                _alivePlayers.Add(player);
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
            _alivePlayers.Remove((Player)player);
            if (_alivePlayerCount == 0) EndGame?.Invoke();
        }
    }
}
