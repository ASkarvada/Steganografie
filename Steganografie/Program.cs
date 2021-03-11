using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Steganografie
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";

            foreach (var arg in args)
            {
                input += arg;
            }
            
            bool b = Check(input, out string text, out string address, out bool hide);
            if (b)
            {
                if(hide) Hide(text, address);
                else Show(address);
            }
            else Console.WriteLine("Chybný formát zadání");

            Console.ReadLine();
        }

        private static void Show(string address)
        {
            Bitmap img = new Bitmap(address);
            string message = "";

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    if (i < 1 && j < 10)
                    {
                        int value = pixel.B;
                        char c = Convert.ToChar(value);
                        string letter = System.Text.Encoding.ASCII.GetString(new byte[] { Convert.ToByte(c) });

                        message += letter;
                    }
                }
            }

            Console.WriteLine(message); 
        }

        private static void Hide(string text, string address)
        {
            Bitmap img = new Bitmap(address);

            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if(i < 1 && j < text.Length)
                    {
                        char letter = Convert.ToChar(text.Substring(j, 1));
                        int value = Convert.ToInt32(letter);
                        img.SetPixel(i, j, Color.FromArgb(pixel.R, pixel.G, value));
                    }
                }
            }

            img.Save(address);
        }

        private static bool Check(string input, out string text, out string address, out bool hide)
        {
            text = "";
            string[] array = input.Split(' ');
            if (array[0] + " " + array[1] == "stega --hide")
            {
                for (int i = 2; i < array.Length-1; i++)
                {
                    text += " " + array[i];
                }
                address = array[array.Length - 1];
                hide = true;
                return true;
            }
            else if (array[0] + " " + array[1] == "stega --show")
            {
                for (int i = 2; i < array.Length; i++)
                {
                    text += " " + array[i];
                }
                text = "";
                address = array[array.Length - 1];
                hide = false;
                return true;
            }
            else
            {
                text = "";
                address = "";
                hide = false;
                return false;
            }
        }
    }
}
