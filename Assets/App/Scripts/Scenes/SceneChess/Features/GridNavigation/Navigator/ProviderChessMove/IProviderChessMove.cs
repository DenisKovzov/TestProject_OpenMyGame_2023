using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using System.Collections.Generic;
using UnityEngine;

public interface IProviderChessMove
{
    List<Vector2Int> GetMoveList(ChessUnitType unitType, Vector2Int gridSize);
}
