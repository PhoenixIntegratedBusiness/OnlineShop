using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Generator
{
    public static class UniqCodeGenerator
    {
        public static string GeneratUniqCode()
        {
            Random rnd = new Random();
             var rand= rnd.Next(1000000,9000000);
            return rand.ToString();
        }
    }
}
