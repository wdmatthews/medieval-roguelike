using System.Collections.Generic;
using UnityEngine;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Rooms
{
    [AddComponentMenu("Medieval Roguelike/Rooms/Room")]
    public class Room : MonoBehaviour
    {
        [SerializeField] private RegionSO _region;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform[] _enemySpawnPoints;
        [SerializeField] private Collider2D _exitCollider;
        [SerializeField] private string _playerLayerName;

        private List<Enemy> _aliveEnemies;
        private int _playerLayer;
        private System.Action _onNextRoom;

        public int EnemySpawnPointCount => _enemySpawnPoints.Length;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == _playerLayer) GoToNextRoom();
        }

        public Room SpawnRoom(System.Action onNextRoom)
        {
            Room room = Instantiate(this);
            room.name = name;
            room._exitCollider.enabled = false;
            room._aliveEnemies = new List<Enemy>();
            room._playerLayer = LayerMask.NameToLayer(_playerLayerName);
            room._onNextRoom = onNextRoom;
            return room;
        }

        public void SpawnPlayers(List<Player> players)
        {
            foreach (Player player in players)
            {
                player.Spawn(_playerSpawnPoint);
            }
        }

        public void SpawnEnemies(int difficulty)
        {
            List<Enemy> validEnemies = _region.GetValidEnemies(difficulty);
            int validEnemyCount = validEnemies.Count;
            int enemyCount = difficulty * 2;

            for (int i = 0; i < enemyCount; i++)
            {
                Enemy enemy = Instantiate(validEnemies[Random.Range(0, validEnemyCount)], transform);
                enemy.Spawn(_enemySpawnPoints[i], OnEnemyDeath);
                _aliveEnemies.Add(enemy);
            }
        }

        private void OnEnemyDeath(Character enemy)
        {
            _aliveEnemies.Remove((Enemy)enemy);
            if (_aliveEnemies.Count == 0) UnlockExit();
        }

        private void UnlockExit()
        {
            _exitCollider.enabled = true;
        }

        private void GoToNextRoom()
        {
            _onNextRoom?.Invoke();
        }
    }
}
