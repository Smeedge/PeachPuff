using System;

namespace Project1
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Project1 game = new Project1())
            {
                game.Run();
            }
        }
    }
#endif
}

