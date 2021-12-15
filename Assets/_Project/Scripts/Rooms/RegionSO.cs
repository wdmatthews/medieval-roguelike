using System.Collections.Generic;
using UnityEngine;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Rooms
{
    [CreateAssetMenu(fileName = "New Region", menuName = "Medieval Roguelike/Rooms/Region")]
    public class RegionSO : ScriptableObject
    {
        [SerializeField] private Room[] _roomPrefabs;
        [SerializeField] private Enemy[] _enemyPrefabs;

        public Room[] RoomPrefabs => _roomPrefabs;
        public Enemy[] EnemyPrefabs => _enemyPrefabs;

        public List<Enemy> GetValidEnemies(int difficulty)
        {
            List<Enemy> validEnemies = new List<Enemy>();

            foreach (Enemy enemyPrefab in _enemyPrefabs)
            {
                int enemyDifficulty = enemyPrefab.EnemyData.Difficulty;
                if (enemyDifficulty >= difficulty - 2 && enemyDifficulty <= difficulty) validEnemies.Add(enemyPrefab);
            }

            return validEnemies;
        }

        public Room GetNextRoom(Room previousRoom, int difficulty)
        {
            List<Room> validRoomPrefabs = new List<Room>();

            foreach (Room roomPrefab in _roomPrefabs)
            {
                if (!previousRoom || (roomPrefab.name != previousRoom.name
                    && roomPrefab.EnemySpawnPointCount >= difficulty * 2)) validRoomPrefabs.Add(roomPrefab);
            }

            return validRoomPrefabs[Random.Range(0, validRoomPrefabs.Count)];
        }
    }
}
