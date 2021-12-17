using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MedievalRoguelike.Characters;
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
        [SerializeField] private PlayerListSO _alivePlayers;
        [SerializeField] private GameHUDSO _gameHUDReference;
        [SerializeField] private GameOverWindowSO _gameOverWindowReference;

        [System.NonSerialized] private int _difficulty;
        [System.NonSerialized] private int _totalRoomsCleared;
        [System.NonSerialized] private RegionSO _currentRegion;
        [System.NonSerialized] private Room _currentRoom;
        [System.NonSerialized] private int _roomsLeftUntilDifficultyChange;
        [System.NonSerialized] private GameHUD _gameHUD;
        [System.NonSerialized] private GameOverWindow _gameOverWindow;

        public RegionSO[] Regions => _regions;
        public RegionSO CurrentRegion { get => _currentRegion; set => _currentRegion = value; }

        public void StartGame()
        {
            _difficulty = 1;
            _totalRoomsCleared = 0;
            _roomsLeftUntilDifficultyChange = _roomsUntilDifficultyChange;
            _playerManager.EndGame = EndGame;
            if (!_currentRegion) _currentRegion = GetNextRegion();
            SpawnRoom(GetNextRoom());
            _gameHUD = _gameHUDReference.HUD;
            _gameHUD.UpdateDifficulty(_difficulty);
            _gameOverWindow = _gameOverWindowReference.Window;
            _gameOverWindow.Continue = Continue;
        }

        private void SpawnRoom(Room roomPrefab)
        {
            if (_currentRoom) Destroy(_currentRoom.gameObject);
            Room room = roomPrefab.SpawnRoom(NextRoom);
            room.SpawnPlayers(_alivePlayers.Players);
            room.SpawnEnemies(Mathf.Clamp(_difficulty, 1, _maxDifficulty));
            _currentRoom = room;
        }

        private void NextRoom()
        {
            _totalRoomsCleared++;
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
            _gameHUD.Hide();
            (int totalPoints, int totalKills) = _playerManager.GetPointsAndKills();
            _gameOverWindow.Open(_difficulty, _totalRoomsCleared, totalPoints, totalKills);
        }

        private void Continue()
        {
            SceneManager.LoadScene("Character Selection");
        }
    }
}
