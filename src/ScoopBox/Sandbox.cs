﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ScoopBox
{
    public class Sandbox : ISandbox
    {
        private readonly ISandboxProcess _scoopBoxProcess;
        private readonly ISandboxConfigurationBuilder _sandboxConfigurationBuilder;
        private readonly IPackageManager _packageManager;

        public Sandbox()
            : this(new SandboxCmdProcess(), new SandboxConfigurationBuilder(new SandboxConfigurationOptions()), new ScoopPackageManager())
        {
        }

        public Sandbox(SandboxConfigurationOptions options)
            : this(new SandboxCmdProcess(), new SandboxConfigurationBuilder(options), new ScoopPackageManager())
        {
        }

        public Sandbox(ISandboxProcess scoopBoxProcess)
            : this(new SandboxCmdProcess(), new SandboxConfigurationBuilder(new SandboxConfigurationOptions()), new ScoopPackageManager())
        {
        }

        public Sandbox(IPackageManager packageManager)
            : this(new SandboxCmdProcess(), new SandboxConfigurationBuilder(new SandboxConfigurationOptions()), packageManager)
        {
        }

        public Sandbox(ISandboxConfigurationBuilder sandboxConfigurationBuilder)
            : this(new SandboxCmdProcess(), sandboxConfigurationBuilder, new ScoopPackageManager())
        {
        }

        public Sandbox(ISandboxProcess scoopBoxProcess, ISandboxConfigurationBuilder sandboxConfigurationBuilder, IPackageManager packageManager)
        {
            _scoopBoxProcess = scoopBoxProcess ?? throw new ArgumentNullException(nameof(scoopBoxProcess));
            _sandboxConfigurationBuilder = sandboxConfigurationBuilder ?? throw new ArgumentNullException(nameof(sandboxConfigurationBuilder));
            _packageManager = packageManager ?? throw new ArgumentNullException(nameof(packageManager));
        }

        public async Task Run()
        {
            string sandboxConfiguration = _sandboxConfigurationBuilder.Build();
            await GenerateSandboxConfiguration(sandboxConfiguration);

            await _scoopBoxProcess.Start();
        }

        public Task Run(IEnumerable<string> applications)
        {
            throw new System.NotImplementedException();
        }

        public Task Run(FileStream scriptBefore, IEnumerable<string> applications)
        {
            throw new System.NotImplementedException();
        }

        public Task Run(FileStream scriptBefore, IEnumerable<string> applications, FileStream scriptAfter)
        {
            throw new System.NotImplementedException();
        }

        private async Task GenerateSandboxConfiguration(string configuration)
        {
            // TODO: Fix this since it is used in many places.
            using (StreamWriter writer = File.CreateText($@"{Directory.CreateDirectory($"{Path.GetTempPath()}/{Constants.SandboxFolderName}").FullName}\{Constants.SandboxScriptName}"))
            {
                await writer.WriteAsync(configuration);
            }
        }
    }
}
