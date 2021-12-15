using System.Collections.Generic;
using UnityEngine;
using MedievalRoguelike.Rooms;
using MedievalRoguelike.UI;

namespace MedievalRoguelike.Managers
{
    [CreateAssetMenu(fileName = "Game Manager", menuName = "Medieval Roguelike/Managers/Game Manager")]
    public class GameManagerSO : ScriptableObject
    {
        [SerializeField] private RegionSO[] _regions;
        [SerializeField] private int _maxDifficulty;
        [SerializeField] private int _roomsUntilDifficultyChange;
        [SerializeField] private PlayerManagerSO _playerManager;
        [SerializeField] private GameHUDSO _gameHUDReference;

        [System.NonSerialized] private int _difficulty;
        [System.NonSerialized] private RegionSO _currentRegion;
        [System.NonSerialized] private Room _currentRoom;
        [System.NonSerialized] private int _roomsLeftUntilDifficultyChange;
        [System.NonSerialized] private GameHUD _gameHUD;

        public void StartGame()
        {
            _difficulty = 1;
            _roomsLeftUntilDifficultyChange = _roomsUntilDifficultyChange;
            _playerManager.EndGame = EndGame;
            _currentRegion = GetNextRegion();
            SpawnRoom(GetNextRoom());
            _gameHUD = _gameHUDReference.HUD;
            _gameHUD.UpdateDifficulty(_difficulty);
        }

        private void SpawnRoom(Room roomPrefab)
        {
            if (_currentRoom) Destroy(_currentRoom.gameObject);
            Room room = roomPrefab.SpawnRoom(NextRoom);
            room.SpawnPlayers(_playerManager.AlivePlayers);
            room.SpawnEnemies(Mathf.Clamp(_difficulty, 1, _maxDifficulty));
            _currentRoom = room;
        }

        private void NextRoom()
        {
            _roomsLeftUntilDifficultyChange--;
            if (_roomsLeftUntilDifficultyChange == 0) IncreaseDifficulty();
            SpawnRoom(GetNextRoom());
        }

        private void IncreaseDifficulty()
        {
            _difficulty++;
            _roomsLeftUntilDifficultyChange = _roomsUntilDifficultyChange;
            _currentRegion = GetNextRegion();
            _gameHUD.UpdateDifficulty(_difficulty);
        }

        private RegionSO GetNextRegion()
        {
            List<RegionSO> validRegions = new List<RegionSO>();

            foreach (RegionSO region in _regions)
            {
                if (!_currentRegion || region.name != _currentRegion.name) validRegions.Add(region);
            }

            return validRegions[Random.Range(0, validRegions.Count)];
        }

        private Room GetNextRoom()
        {
            return _currentRegion.GetNextRoom(_currentRoom, _difficulty);
        }

        private void EndGame()
        {
            _playerManager.TurnOffPlayers();
            Destroy(_currentRoom.gameObject);
        }
    }
}
