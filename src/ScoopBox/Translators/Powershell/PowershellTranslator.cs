﻿using ScoopBox.Abstractions;
using System;
using System.IO;
using System.Text;

namespace ScoopBox.Translators.Powershell
{
    public class PowershellTranslator : IPowershellTranslator
    {
        private readonly string[] _argumentsAfter;
        private readonly Func<long> _getTicks;

        public PowershellTranslator()
            : this(null)
        {
        }

        public PowershellTranslator(string[] argumentsAfter)
            : this(argumentsAfter, DateTimeAbstractions.GetTicks)
        {
        }

        internal PowershellTranslator(string[] argumentsAfter, Func<long> getTicks)
        {
            if (getTicks == null)
            {
                throw new ArgumentNullException(nameof(getTicks));
            }

            _argumentsAfter = argumentsAfter;
            _getTicks = getTicks;
        }

        public string Translate(FileSystemInfo file, IOptions options)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            string sandboxScriptFileFullName = Path.Combine(options.RootSandboxFilesDirectoryLocation, file.Name);

            StringBuilder sbPowershellCommandBuilder = new StringBuilder()
                .Append($@"powershell.exe -ExecutionPolicy Bypass -File { sandboxScriptFileFullName }")
                .Append(" ")
                .Append($@"3>&1 2>&1 > ""{Path.Combine(options.SandboxDesktopLocation, $"Log_{_getTicks()}.txt")}""");

            if (_argumentsAfter?.Length > 0)
            {
                sbPowershellCommandBuilder.Append(" ").Append(string.Join(" ", _argumentsAfter));
            }

            return sbPowershellCommandBuilder.ToString();
        }
    }
}
