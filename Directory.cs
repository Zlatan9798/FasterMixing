﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FasterMixing
{
    public class Directory
    {
        public static string modDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
