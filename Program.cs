using System;

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
        public static bool UserWon { get; set; }
        public static int UserMovesCount { get; set; }
        public static int MachineMovesCount { get; set; }


        static void Main(string[] args)
        {           
            try
            {
                while(GameGoesOn)
                {
                    Console.Clear();
                    if(UserTurn)
                    {
                        CreateBoard();
                        var correctInput = true;
                        int x =0,y=0;
                        while(correctInput)
                        {
                            var res = GetNumber();
                            x = res.Item1;
                            y = res.Item2;
                            correctInput = IndexReserved(x,y);
                        }
                        WriteOnBoard(x,y);
                    }
                    else
                        MachineTurn();
                    if(CheckForWin())
                    {
                        GameGoesOn = false;
                        UserWon = UserTurn;
                    }
                    ChangeTurn();
                }
                Console.Clear();
                CreateBoard();
                Console.WriteLine($"\n user won : {UserWon}");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static bool CheckForWin(string[,] board = null)
        {
            board ??= Board;
            // int jump = 1;
            // check for inline win
            for(var i = 1; i < 10;i += 3)
            {
                int x =0,y=0;
                (x,y) = ConvertNumberToBoardIndex(i);
                var current = Board[x,y];
                var currentPlus = Board[x,y + 1];
                var currentPlusPlus = Board[x,y + 2];
                if(current != "" && current == currentPlus && currentPlus == currentPlusPlus)
                    return true;
                
            }
            // vertical check for win
            for(var i = 1; i < 4;i++)
            {
                int x =0,y=0;
                (x,y) = ConvertNumberToBoardIndex(i);
                var current = Board[x,y];

                // next
                int currentPlusX,currentPlusY;
                (currentPlusX,currentPlusY) = ConvertNumberToBoardIndex(i + 3);
                var currentPlus = Board[currentPlusX,currentPlusY];
              
                // last
                int currentPlusPlusX,currentPlusPlusY;
                (currentPlusPlusX,currentPlusPlusY) = ConvertNumberToBoardIndex(i + 6);
                var currentPlusPlus = Board[currentPlusPlusX,currentPlusPlusY];
                if(current != "" && current == currentPlus && currentPlus == currentPlusPlus)
                    return true;
                
            }
            // edge check for win
            for(var i = 1; i < 4;i += 2)
            {
                //the first one
                int x =0,y=0;
                (x,y) = ConvertNumberToBoardIndex(i);
                var current = Board[x,y];

                // next
                int currentPlusX,currentPlusY;
                (currentPlusX,currentPlusY) = ConvertNumberToBoardIndex(i == 1 ? i + 4 : i + 2);
                var currentPlus = Board[currentPlusX,currentPlusY];

                // last one
                int currentPlusPlusX,currentPlusPlusY;
                (currentPlusPlusX,currentPlusPlusY) = ConvertNumberToBoardIndex(i == 1 ? i + 8 : i + 4);
                var currentPlusPlus = Board[currentPlusPlusX,currentPlusPlusY];
                if(current != "" && current == currentPlus && currentPlus == currentPlusPlus)
                    return true;
            }

            return false;
        }

        private static void WriteOnBoard(int x, int y)
        {
            if (UserTurn)
                Board[x,y] = "X";
            else 
                Board[x,y] = "O";
            if(UserTurn)
                UserMovesCount++;
            else
                MachineMovesCount++;
        }

        private static (int,int) GetNumber()
        {
            Console.WriteLine("\nEnter number : ");
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
            // var x = 0;
            // var y = 0;
            // if (number < 4)
            //     x = 0;
            // else if (number < 7)
            //     x = 1;
            // else 
            //     x = 2;
            // y = 9 - ( (9 / number) * number);
            // //Console.WriteLine($"x : {x} and y is : {y}");
            // return (x,y);
            if (number == 1)
                return (0,0);
            else if (number == 2)
                return (0,1);
            else if (number == 3)
                return (0,2);
            else if (number == 4)
                return (1,0);
            else if (number == 5)
                return (1,1);
            else if (number == 6)
                return (1,2);
            else if (number == 7)
                return (2,0);
            else if (number == 8)
                return (2,1);
            else if (number == 9)
                return (2,2);
            throw new Exception("Unexpected number");
        }

        private static void MachineTurn()
        {
            var board = Board;
            var rnd = new Random();
            bool jobDone = false;
            var moves = 20;
            var usedNumbers = "";
            do{
                var thisRnd = rnd.Next(1,10);
                if(!usedNumbers.Contains(thisRnd.ToString()))
                {
                    usedNumbers += thisRnd;
                    var (x,y) = ConvertNumberToBoardIndex(thisRnd);
                    jobDone = !IndexReserved(x,y,board);
                    if(jobDone){
                        board[x,y] = "O";
                        var canWin = CheckForWin(board);
                        if(canWin || moves == 1){
                            WriteOnBoard(x,y);
                        }
                    }
                    moves--;
                }
               
            }while(!jobDone && moves != 0);
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