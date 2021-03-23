using System;
using System.Collections.Generic;

namespace tic_tac_toe
{
    class Program
    {
        public static bool UserTurn  = true;
        public static bool GameGoesOn  = true;
        public static string[,] Board = new string[,] 
        { 
            { "", "","" },
            { "", "","" },
            { "", "","" }
        };
        public static Dictionary<string,(int,int)> FreeSpawns = new Dictionary<string, (int, int)>();
        public static bool UserWon = false;
        public static int UserMovesCount = 0;
        public static int MachineMovesCount = 0;
        public static string UserCharacter = "X";
        public static string MachineCharacter = "O";

        static void Main(string[] args)
        {           
            try
            {
                while(GameGoesOn)
                {
                    Console.Clear();
                    CalculateFreeIndexes();
                    if(UserTurn)
                    {
                        CreateBoard();
                        var correctInput = true;
                        int x = 0,y = 0;
                        while(correctInput)
                        {
                            (x,y) = GetNumber();
                            correctInput = IndexReserved(x,y);
                        }
                        WriteOnBoard(x,y);
                    }
                    else
                        MachineTurn();
                    if(CheckForWin())
                    {
                        FinishGame();
                    }
                    ChangeTurn();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }

        private static void CalculateFreeIndexes()
        {
            FreeSpawns.Clear();
            for(var i = 0; i < 3; i++)
            {
                for(var j = 0; j < 3;j++)
                {
                    if(Board[i,j] == string.Empty)
                        FreeSpawns.Add(Guid.NewGuid().ToString(),(i,j));
                }
            }
        }

        private static void FinishGame()
        {
            GameGoesOn = false;
            UserWon = UserTurn;
            Console.Clear();
            CreateBoard();
            Console.WriteLine($"\n user won : {UserWon}");
        }

        private static bool CheckForWin(string[,] board = null)
        {
            board ??= Board;
            for(var i = 1; i < 10;i += 3)
            {
                var (x,y) = ConvertNumberToBoardIndex(i);
                if(board[x,y] != "" && board[x,y] ==  board[x,y + 1] &&  board[x,y + 1] == board[x,y + 2])
                    return true;
            }
            // vertical check for win
            for(var i = 1; i < 4;i++)
            {
                var (x,y) = ConvertNumberToBoardIndex(i);
                var current = board[x,y];
                // next
                var (currentPlusX,currentPlusY) = ConvertNumberToBoardIndex(i + 3);
                var currentPlus = board[currentPlusX,currentPlusY];
                // last
                var (currentPlusPlusX,currentPlusPlusY) = ConvertNumberToBoardIndex(i + 6);
                if(current != "" && current == currentPlus && currentPlus == board[currentPlusPlusX,currentPlusPlusY])
                    return true;
                
            }
            // edge check for win
            for(var i = 1; i < 4;i += 2)
            {
                //the first one
                var (x,y) = ConvertNumberToBoardIndex(i);
                var current = board[x,y];
                // next
                var (currentPlusX,currentPlusY) = ConvertNumberToBoardIndex(i == 1 ? i + 4 : i + 2);
                var currentPlus = board[currentPlusX,currentPlusY];
                // last one
                var (currentPlusPlusX,currentPlusPlusY) = ConvertNumberToBoardIndex(i == 1 ? i + 8 : i + 4);
                if(current != "" && current == currentPlus && currentPlus ==  board[currentPlusPlusX,currentPlusPlusY])
                    return true;
            }
            return false;
        }

        private static void WriteOnBoard(int x, int y)
        {
            if (UserTurn)
                Board[x,y] = UserCharacter;
            else 
                Board[x,y] = MachineCharacter;
            if(UserTurn)
                UserMovesCount++;
            else
                MachineMovesCount++;
        }

        private static (int,int) GetNumber()
        {
            Console.WriteLine("\nEnter number :");
            Console.Write("> ");
            int x =0,y=0;
            if (int.TryParse(Console.ReadLine(),out var number) && number > 0 && number < 10)
            {
                (x,y) = ConvertNumberToBoardIndex(number);
            } else {
                Console.WriteLine("Please enter valid number between 0 and 9");
            }
            return (x,y);
        }

        private static bool IndexReserved(int x,int y,string[,] board = null)
        {
            board ??= Board;
            return board[x,y] != "";
        }

        private static (int,int) ConvertNumberToBoardIndex(int number)
        {
            switch (number)
            {
                case 1:
                    return (0, 0);
                case 2:
                    return (0, 1);
                case 3:
                    return (0, 2);
                case 4:
                    return (1, 0);
                case 5:
                    return (1, 1);
                case 6:
                    return (1, 2);
                case 7:
                    return (2, 0);
                case 8:
                    return (2, 1);
                case 9:
                    return (2, 2);
            }
            throw new Exception("Unexpected number");
        }

        private static void Clone(string[,] to,string[,] from = null)
        {
            from ??= Board;
            for(var i = 0; i < 3; i++)
            {
                for(var j = 0; j < 3;j++)
                {
                    to[i,j] = from[i,j];
                }
            }
        }

        private static void MachineTurn()
        {
            var board = new string[3,3];
            Clone(board);
            // check if user can with with another move : if yes  we will take that peice :D kalak rashti for the win 
            foreach (var item in FreeSpawns)
            {
                board[item.Value.Item1,item.Value.Item2] = UserCharacter;
                var res = CheckForWin(board);
                if(res){
                    WriteOnBoard(item.Value.Item1,item.Value.Item2);
                    return;
                }
            }
            Clone(board);
            var rnd = new Random();
            var moves = 20;
            int finalX = 0,finalY = 0;
            do{
                var thisRnd = rnd.Next(1,10);
                var (x,y) = ConvertNumberToBoardIndex(thisRnd);
                if(!IndexReserved(x,y,board)){
                    finalX = x;
                    finalY = y;
                    board[x,y] = MachineCharacter;
                    var canWin = CheckForWin(board);
                    if(canWin || moves == 0){
                        break;
                    }
                    moves--;
                }
            }while(moves >= 0);
            WriteOnBoard(finalX,finalY);
        }

        private static void ChangeTurn()
        {
            UserTurn = !UserTurn;
        }

        static void CreateBoard()
        {
            Console.Write("\n");
            Console.Write($"User Moves : {UserMovesCount} \t Machine Moves : {MachineMovesCount}\n\n");
            for (var i = 0; i < 3;i++)
            {
                Console.Write("\t");
                for (var j = 0;j < 3;j++)
                {
                    var c = Board[i,j];
                    Console.Write(c);
                    if(string.IsNullOrEmpty(c))
                        Console.Write(" ");
                    if(j != 2)
                        Console.Write("   |   ");
                }
                if(i != 2)
                    Console.Write("\n\t----+-------+----\n");
            }
        }
    }
}
