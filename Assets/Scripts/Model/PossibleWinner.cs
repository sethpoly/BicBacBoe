using Pieces;

public struct PossibleWinner
{
    public PossibleWinner(bool _didWin = false, PieceType? _pieceType = null)
    {
        didWin = _didWin;
        pieceType = _pieceType;
    }

    public bool didWin { get; }
    public PieceType? pieceType { get; }

    public override string ToString()
    {
        return $"didWin = {didWin}, pieceType = {pieceType}";
    }
} 