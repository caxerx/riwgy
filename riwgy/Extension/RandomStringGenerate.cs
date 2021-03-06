﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace riwgy.Extension
{
    public static class RandomStringGenerate
    {
        public static string GenerateCoupon(this string content, int length)
        {
            Random random = new Random();
            string characters = content;
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }
    }
}
