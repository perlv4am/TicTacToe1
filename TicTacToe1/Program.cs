using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe1
{
    class Program
    {
        static void Main(string[] args)
        {
            TicTacToe_Game game1 = new TicTacToe_Game(dimension:3);
            game1.Play();
        }
    } 
}
