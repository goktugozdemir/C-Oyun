using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Xml.Linq;
using static System.Convert;

namespace Packt.Shared
{
    
        public static class Protector
        {
             public static readonly byte[] salt = Encoding.Unicode.GetBytes("8AB5RWV1");
        public static readonly int iterations=2000;
        }
    
}
