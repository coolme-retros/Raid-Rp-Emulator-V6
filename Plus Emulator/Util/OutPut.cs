﻿using System;

namespace Plus
{
    public class Out
    {
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="header">The header.</param>
        /// <param name="color">The color.</param>
        public static void WriteLine(string format, string header = "", ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");
            if (header != "")
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(header);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("] ");
            }

            Console.Write("");
            Console.ForegroundColor = color;
            Console.WriteLine(format);
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        /// <summary>
        /// Writes the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="header">The header.</param>
        /// <param name="color">The color.</param>
        public static void Write(string format, string header = "", ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[" + DateTime.Now + "] ");
            if (header != "")
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write(header);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("] ");
            }

            Console.Write(">> ");
            Console.ForegroundColor = color;
            Console.Write(format);
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }
    }
}