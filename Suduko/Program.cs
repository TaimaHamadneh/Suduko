int[,] inputs = new int[,]
        {
            {9, 6, 2, 1, 4, 7, 3, 7, 8},
            {1, 8, 5, 6, 7, 3, 4, 2, 9},
            {3, 7, 4, 2, 9, 8, 5, 6, 1},
            {5, 3, 1, 7, 6, 2, 9, 8, 4},
            {6, 9, 4, 3, 8, 1, 2, 5, 7},
            {8, 2, 7, 4, 5, 9, 6, 1, 3},
            {4, 9, 6, 5, 1, 7, 8, 3, 2},
            {2, 1, 8, 9, 3, 6, 7, 4, 5},
            {7, 5, 3, 8, 2, 4, 1, 9, 6},
        };

var sudoku = new Sudoku(inputs);
sudoku.PrintBoard();

Console.WriteLine($"\nValue at (5,5): {sudoku[5, 5]}");
sudoku[5, 5] = -2; // Invalid -> ignored
Console.WriteLine($"Value at (5,5) after invalid change: {sudoku[5, 5]}");

Console.WriteLine($"\nRow 1 valid? {sudoku.IsRowValid(0)}");
Console.WriteLine($"Column 1 valid? {sudoku.IsColumnValid(0)}");
Console.WriteLine($"3x3 Box (0,0) valid? {sudoku.IsBoxValid(0, 0)}");




public class Sudoku
{
    private readonly int[,] _matrix;
    private readonly bool[,] _isOriginal; // To track original (locked) cells

    public int Size => _matrix.GetLength(0);

    /// Indexer for accessing or modifying Sudoku cells.
    /// Prevents setting invalid or locked cells.
    public int this[int row, int col]
    {
        get
        {
            if (!IsInRange(row, col))
                return -1;
            return _matrix[row, col];
        }
        set
        {
            if (!IsInRange(row, col) || _isOriginal[row, col])
                return;

            if (value < 1 || value > 9)
                return;

            _matrix[row, col] = value;
        }
    }



    /// Initializes a new Sudoku board.
    public Sudoku(int[,] matrix)
    {
        if (matrix.GetLength(0) != 9 || matrix.GetLength(1) != 9)
            throw new ArgumentException("Sudoku board must be 9x9.");

        _matrix = (int[,])matrix.Clone();
        _isOriginal = new bool[9, 9];

        // Mark initial numbers as locked
        for (int r = 0; r < 9; r++)
            for (int c = 0; c < 9; c++)
                if (_matrix[r, c] != 0)
                    _isOriginal[r, c] = true;
    }

    private bool IsInRange(int row, int col) =>
        row >= 0 && row < 9 && col >= 0 && col < 9;

    public bool IsRowValid(int row)
    {
        bool[] seen = new bool[10];
        for (int c = 0; c < 9; c++)
        {
            int val = _matrix[row, c];
            if (val == 0) continue;
            if (seen[val]) return false;
            seen[val] = true;
        }
        return true;
    }

    public bool IsColumnValid(int col)
    {
        bool[] seen = new bool[10];
        for (int r = 0; r < 9; r++)
        {
            int val = _matrix[r, col];
            if (val == 0) continue;
            if (seen[val]) return false;
            seen[val] = true;
        }
        return true;
    }

    public bool IsBoxValid(int startRow, int startCol)
    {
        bool[] seen = new bool[10];
        for (int r = 0; r < 3; r++)
            for (int c = 0; c < 3; c++)
            {
                int val = _matrix[startRow + r, startCol + c];
                if (val == 0) continue;
                if (seen[val]) return false;
                seen[val] = true;
            }
        return true;
    }


    /// Prints the Sudoku board in a formatted grid.
    public void PrintBoard()
    {
        Console.WriteLine("╔═════════════════════════╗");
        for (int r = 0; r < 9; r++)
        {
            Console.Write("║ ");
            for (int c = 0; c < 9; c++)
            {
                Console.Write(_matrix[r, c] + " ");
                if ((c + 1) % 3 == 0)
                    Console.Write("│ ");
            }
            Console.WriteLine("║ ");
            if ((r + 1) % 3 == 0 && r != 8)
                Console.WriteLine("╟─────────────────────────╢");
        }
        Console.WriteLine("╚═════════════════════════╝");
    }
}
