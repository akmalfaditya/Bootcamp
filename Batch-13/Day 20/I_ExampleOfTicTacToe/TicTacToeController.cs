namespace TicTacToeLib;

class TicTacToeController
{
    private char[] board = new char[9];
    private char currentPlayer = 'X';

    public TicTacToeController()
    {
        ResetBoard();
    }

    public void ResetBoard()
    {
        for (int i = 0; i < board.Length; i++)
            board[i] = ' ';
        currentPlayer = 'X';
    }

    public bool MakeMove(int position)
    {
        if (position < 0 || position >= 9 || board[position] != ' ')
            return false;

        board[position] = currentPlayer;
        SwitchPlayer();
        return true;
    }

    public char GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public char CheckWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i * 3] != ' ' && board[i * 3] == board[i * 3 + 1] && board[i * 3] == board[i * 3 + 2])
                return board[i * 3];
            if (board[i] != ' ' && board[i] == board[i + 3] && board[i] == board[i + 6])
                return board[i];
        }
        if (board[0] != ' ' && board[0] == board[4] && board[0] == board[8])
            return board[0];
        if (board[2] != ' ' && board[2] == board[4] && board[2] == board[6])
            return board[2];

        if (!board.Contains(' '))
            return 'T';

        return ' ';
    }

    private void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
    }
    public char GetBoardPosition(int position)
    {
        if (position < 0 || position >= 9)
            throw new ArgumentOutOfRangeException("Invalid board position.");

        return board[position];
    }
}

