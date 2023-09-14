using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Piece;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        private IProviderChessMove _providerChessMove;
        public ChessGridNavigator(IProviderChessMove providerChessMove)
        {
            _providerChessMove = providerChessMove;
        }

        public List<Vector2Int> FindPath(ChessUnitType unitType, Vector2Int startPosition, Vector2Int targetPosition, ChessGrid grid)
        {
            Dictionary<Vector2Int, Vector2Int> map = new Dictionary<Vector2Int, Vector2Int>();

            Queue<Vector2Int> openList = new Queue<Vector2Int>();
            ISet<Vector2Int> closedList = new HashSet<Vector2Int>();

            openList.Enqueue((startPosition));
            closedList.Add(startPosition);

            List<Vector2Int> moves = _providerChessMove.GetMoveList(unitType, grid.Size);

            while (openList.Count > 0)
            {
                Vector2Int position = openList.Dequeue();


                foreach (var move in moves)
                {
                    var newPosition = position + move;

                    if (!CanMoveTo(newPosition, grid) || closedList.Contains(newPosition))
                        continue;

                    if (unitType != ChessUnitType.Knight && isPathBlockedByPieces(position, newPosition, grid))
                        continue;

                    if (newPosition == targetPosition)
                    {
                        map[newPosition] = position;

                        return GetPath(map, startPosition, targetPosition);
                    }

                    openList.Enqueue(newPosition);
                    closedList.Add(newPosition);

                    map[newPosition] = position;
                }
            }

            return null;
        }

        private List<Vector2Int> GetPath(Dictionary<Vector2Int, Vector2Int> map, Vector2Int startPosition, Vector2Int targetPosition)
        {
            List<Vector2Int> path = new List<Vector2Int>() { targetPosition };

            var position = map[targetPosition];

            while (position != startPosition)
            {
                path.Add(position);
                position = map[position];
            }

            path.Reverse();
            return path;
        }

        private bool CanMoveTo(Vector2Int position, ChessGrid grid)
        {
            if (position.x < 0 || position.x >= grid.Size.x ||
                position.y < 0 || position.y >= grid.Size.y)
            {
                return false;
            }

            return grid.Get(position) == null;
        }

        private bool isPathBlockedByPieces(Vector2Int startPosition, Vector2Int endPosition, ChessGrid grid)
        {
            int deltaX, deltaY;

            int count = Math.Max(Math.Abs(endPosition.x - startPosition.x), Math.Abs(endPosition.y - startPosition.y));
            deltaX = Math.Sign(endPosition.x - startPosition.x);
            deltaY = Math.Sign(endPosition.y - startPosition.y);

            int x, y;

            x = startPosition.x + deltaX;
            y = startPosition.y + deltaY;

            for (int i = 0; i < count; i++)
            {
                if (grid.Get(new Vector2Int(x, y)) != null)
                    return true;

                x += deltaX;
                y += deltaY;
            }

            return false;
        }
    }
}