using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Map
{
    public class MapGenerator : MonoBehaviour
    {
        private const float ChunkWidth = 46.1f;
        private const float PlayerSpawnOffsetY = -10.74f;

        [SerializeField] private List<Chunk> _mapChunks;

        private Player.Player _player;
        private Chunk _centerChunk;
        private Chunk _leftChunk;
        private Chunk _rightChunk;

        [Inject]
        public void Construct(Player.Player player)
        {
            _player = player;
        }

        public void Initialize()
        {
            Deactivate();
            var startChunk = GetRandomInactiveChunk();
            var startPosition = Vector3.zero;

            _player.transform.position = startPosition + new Vector3(0, PlayerSpawnOffsetY, 0);

            ActivateChunk(startChunk, startPosition);
            ActivateAdjacentChunks(startChunk);

            foreach (var chunk in _mapChunks)
            {
                chunk.OnPlayerEnter += HandleChunkEnter;
                chunk.OnPlayerStay += HandleChunkStayEvent;
            }
        }

        private void Deactivate()
        {
            foreach (var chunk in _mapChunks)
            {
                chunk.Deactivate();
            }
        }

        private Chunk GetRandomInactiveChunk()
        {
            var inactiveChunks = _mapChunks.FindAll(chunk => !chunk.IsActive);
            return inactiveChunks[Random.Range(0, inactiveChunks.Count)];
        }

        private void ActivateChunk(Chunk chunk, Vector3 position)
        {
            chunk.Activate(position);
        }

        private void DeactivateChunk(Chunk chunk)
        {
            chunk.Deactivate();
        }

        private void ActivateAdjacentChunks(Chunk centerChunk)
        {
            var centerPosition = centerChunk.transform.position;
            var rightPosition = centerPosition + Vector3.right * ChunkWidth;
            _rightChunk = GetRandomInactiveChunk();
            ActivateChunk(_rightChunk, rightPosition);

            var leftPosition = centerPosition - Vector3.right * ChunkWidth;
            _leftChunk = GetRandomInactiveChunk();
            ActivateChunk(_leftChunk, leftPosition);

            _centerChunk = centerChunk;
        }

        private void HandleChunkEnter(Chunk triggeredChunk)
        {
            if (triggeredChunk == _centerChunk)
                return;

            if (triggeredChunk == _rightChunk)
            {
                var newRightPosition = _rightChunk.transform.position + Vector3.right * ChunkWidth;
                var newRightChunk = GetRandomInactiveChunk();
                ActivateChunk(newRightChunk, newRightPosition);

                DeactivateChunk(_leftChunk);
                
                _leftChunk = _centerChunk;
                _centerChunk = _rightChunk;
                _rightChunk = newRightChunk;
            }
            
            else if (triggeredChunk == _leftChunk)
            {
                var newLeftPosition = _leftChunk.transform.position - Vector3.right * ChunkWidth;
                var newLeftChunk = GetRandomInactiveChunk();
                
                ActivateChunk(newLeftChunk, newLeftPosition);
                DeactivateChunk(_rightChunk);
                
                _rightChunk = _centerChunk;
                _centerChunk = _leftChunk;
                _leftChunk = newLeftChunk;
            }
        }

        private void HandleChunkStayEvent(Chunk chunk)
        {
            if(chunk != _centerChunk)
                HandleChunkEnter(chunk);
        }
    }
}