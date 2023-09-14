using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProviderChessMove : IProviderChessMove
{
    private Dictionary<ChessUnitType, List<Vector2Int>> _chessTypeDataMove;

    private Vector2Int _gridSize;


    public List<Vector2Int> GetMoveList(ChessUnitType unitType, Vector2Int gridSize)
    {
        if (_gridSize != gridSize)
        {
            _gridSize = gridSize;
            Initialize();
        }

        return _chessTypeDataMove[unitType];
    }

    private void Initialize()
    {
        _chessTypeDataMove = new Dictionary<ChessUnitType, List<Vector2Int>>
        {
            { ChessUnitType.Pon, GetPonMoves() },
            { ChessUnitType.King, GetKingMoves() },
            { ChessUnitType.Queen, GetQueenMoves() },
            { ChessUnitType.Rook, GetRookMoves() },
            { ChessUnitType.Knight, GetKnightMoves() },
            { ChessUnitType.Bishop, GetBishoptMoves() },
        };
    }

    private List<Vector2Int> GetPonMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        return moves;
    }

    private List<Vector2Int> GetKingMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>
        {
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(1, -1),
            new Vector2Int(1, 1),
	    new Vector2Int(1, 0)
        };

        return moves;
    }

    private List<Vector2Int> GetQueenMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        moves.AddRange(GetRookMoves());
        moves.AddRange(GetBishoptMoves());

        return moves;

    }
    private List<Vector2Int> GetRookMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        for (int i = -_gridSize.x; i <= _gridSize.x; i++)
        {
            if (i != 0)
            {
                moves.Add(new Vector2Int(i, 0));
                moves.Add(new Vector2Int(0, i));
            }
        }

        return moves;
    }
    private List<Vector2Int> GetKnightMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>
        {
            new Vector2Int(2,1),
            new Vector2Int(1,2),
            new Vector2Int(-2,1),
            new Vector2Int(-1,2),
            new Vector2Int(2,-1),
            new Vector2Int(1,-2),
            new Vector2Int(-2,-1),
            new Vector2Int(-1,-2),
        };

        return moves;
    }
    private List<Vector2Int> GetBishoptMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        for (int i = -_gridSize.x; i <= _gridSize.x; i++)
        {
            if (i != 0)
            {
                moves.Add(new Vector2Int(i, i));
                moves.Add(new Vector2Int(-i, i));
            }
        }

        return moves;
    }
}
