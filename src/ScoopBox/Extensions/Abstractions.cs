﻿using Microsoft.Extensions.DependencyInjection;
using ScoopBox.Sandbox;
using ScoopBox.Sandbox.Abstract;
using ScoopBox.SandboxProcess;
using ScoopBox.SandboxProcess.Abstract;
using ScoopBox.Scoop;
using ScoopBox.Scoop.Abstract;
using ScoopBox.Scripts.InstallerScripts;
using ScoopBox.Scripts.InstallerScripts.Abstract;
using ScoopBox.Scripts.SandboxScripts;
using ScoopBox.Scripts.SandboxScripts.Abstract;

namespace ScoopBox.Extensions
{
    public static class Abstractions
    {
        public static IServiceCollection UseScoopBox(this IServiceCollection services)
        {
            services.AddScoped<IScoopScript, ScoopScript>();
            services.AddScoped<ISandboxScript, SandboxScript>();

            services.AddScoped<IExecutionPolicyCommandBuilder, ExecutionPolicyCommandBuilder>();
            services.AddScoped<IScoopInstallerBuilder, ScoopInstallerBuilder>();
            services.AddScoped<IScoopBucketsBuilder, ScoopBucketsBuilder>();
            services.AddScoped<IAppInstallerBuilder, AppInstallerBuilder>();
            services.AddScoped<IInstallerScriptBuilder, InstallerScriptBuilder>();

            services.AddScoped<ISandboxScriptBuilder, SandboxScriptBuilder>();
            services.AddScoped<IExecutionScriptCommandBuilder, ExecutionScriptCommandBuilder>();
            services.AddScoped<IExecutionPolicyCommandBuilder, ExecutionPolicyCommandBuilder>();
            services.AddScoped<IConfigurationBuilder, ConfigurationBuilder>();
            services.AddScoped<ICommandBuilder, CommandBuilder>();
            services.AddScoped<IMappedFoldersBuilder, MappedFoldersBuilder>();

            services.AddScoped<IScoopBoxProcess, ScoopBoxProcess>();

            return services;
        }
    }
}
