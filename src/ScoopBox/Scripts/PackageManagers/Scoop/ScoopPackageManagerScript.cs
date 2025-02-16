using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("ScoopBox.Test")]
namespace ScoopBox
{
    /// <summary>
    /// Represents Scoop package manager.
    /// This script is generated automatically based on user input.
    /// </summary>
    public class ScoopPackageManagerScript : IPackageManagerScript
    {
        private string _packageManagerScriptName;
        private readonly StringBuilder _sbScoopPackageManagerBuilder;
        private readonly Func<string, byte[], CancellationToken, Task> _writeAllBytesAsync;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoopPackageManagerScript"/> class.
        /// </summary>
        /// <param name="applications">
        /// Applications that will be installed using this package manager.
        /// </param>
        public ScoopPackageManagerScript(IEnumerable<string> applications)
            : this(
                  applications,
                  new PowershellTranslator(),
                  $"{nameof(ScoopPackageManagerScript)}.ps1")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoopPackageManagerScript"/> class.
        /// </summary>
        /// <param name="applications">
        /// Applications that will be installed using this package manager.
        /// </param>
        /// <param name="translator">
        /// Translator that will used to generate command that will be run from powershell.
        /// <para>
        /// The default translate is <see cref="PowershellTranslator"/>.
        /// </para>
        /// <para>
        /// This constructor is defined solely if the user has defined custom translator using <see cref="IPowershellTranslator"/>.
        /// </para>
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null.</exception>
        public ScoopPackageManagerScript(IEnumerable<string> applications, IPowershellTranslator translator)
            : this(
                  applications,
                  translator,
                  $"{nameof(ScoopPackageManagerScript)}.ps1")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoopPackageManagerScript"/> class.
        /// </summary>
        /// <param name="applications">
        /// Applications that will be installed using this package manager.
        /// </param>
        /// <param name="translator">
        /// Translator that will used to generate command that will be run from powershell.
        /// <para>
        /// The default translate is <see cref="PowershellTranslator"/>.
        /// </para>
        /// <para>
        /// This constructor is defined solely if the user has defined custom translator using <see cref="IPowershellTranslator"/>.
        /// </para>
        /// </param>
        /// <param name="scriptName">
        /// The name of the script that will be generated.
        /// <para>The default is ScoopPackageManagerScript.ps1</para>
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null.</exception>
        public ScoopPackageManagerScript(
            IEnumerable<string> applications,
            IPowershellTranslator translator,
            string scriptName)
            : this(
                  applications,
                  translator,
                  scriptName,
                  new StringBuilder(),
                  FileSystemAbstractions.WriteAllBytesAsync)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyPackageManagerScript"/> class.
        /// This constructor is solely for testing purposes and contains framework specific classes that cannot be tested.
        /// </summary>
        /// <param name="applications">
        /// Applications that will be installed using this package manager.
        /// </param>
        /// <param name="translator">
        /// Translator that will used to generate command that will be run from powershell.
        /// <para>
        /// The default translate is <see cref="PowershellTranslator"/>.
        /// </para>
        /// <para>
        /// This constructor is defined solely if the user has defined custom translator using <see cref="IPowershellTranslator"/>.
        /// </para>
        /// </param>
        /// <param name="scriptName">
        /// The name of the script that will be generated.
        /// <para>The default is ScoopPackageManagerScript.ps1</para>
        /// </param>
        /// <param name="writeAllBytesAsync">
        /// Delegate that takes filePath, content and cancellation token as parameter and generate a new file asynchronously.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null.</exception>
        internal ScoopPackageManagerScript(
            IEnumerable<string> applications,
            IPowershellTranslator translator,
            string packageManagerScriptName,
            StringBuilder sbScoopPackageManagerBuilder,
            Func<string, byte[], CancellationToken, Task> writeAllBytesAsync)
        {
            if (applications == null || !applications.Any())
            {
                throw new ArgumentNullException(nameof(applications));
            }

            if (translator == null)
            {
                throw new ArgumentNullException(nameof(translator));
            }

            if (string.IsNullOrWhiteSpace(packageManagerScriptName))
            {
                throw new ArgumentNullException(nameof(packageManagerScriptName));
            }

            if (sbScoopPackageManagerBuilder == null)
            {
                throw new ArgumentNullException(nameof(sbScoopPackageManagerBuilder));
            }

            if (writeAllBytesAsync == null)
            {
                throw new ArgumentNullException(nameof(writeAllBytesAsync));
            }

            Applications = applications;
            Translator = translator;

            _packageManagerScriptName = packageManagerScriptName;
            _sbScoopPackageManagerBuilder = sbScoopPackageManagerBuilder;
            _writeAllBytesAsync = writeAllBytesAsync;
        }

        /// <summary>
        /// Gets or sets the generated script file.
        /// </summary>
        public FileSystemInfo ScriptFile { get; set; }

        /// <summary>
        /// Gets the applications that will be installed using this package manager.
        /// </summary>
        public IEnumerable<string> Applications { get; }

        /// <summary>
        /// Gets the translator that will be used to generate powershell command.
        /// </summary>
        public IPowershellTranslator Translator { get; }

        /// <summary>
        /// Generates a new script in <see cref="IOptions.RootFilesDirectoryLocation"/>.
        /// Points <see cref="ScriptFile"/> to the newly generated script.
        /// The script installs scoop package manager to Windows Sandbox and the <see cref="Applications"/>.
        /// </summary>
        /// <param name="options">
        /// Enables the user to control some aspects of Windows Sandbox.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used to cancel the operation.
        /// </param>
        public async Task Process(IOptions options, CancellationToken cancellationToken = default)
        {
            _sbScoopPackageManagerBuilder.AppendLine(@"Write-Host Setting Execution Policy");
            _sbScoopPackageManagerBuilder.AppendLine("Set-ExecutionPolicy Bypass -Force");
            _sbScoopPackageManagerBuilder.AppendLine(@"Write-Host Installing PWSH 7");
            _sbScoopPackageManagerBuilder.AppendLine("irm https://aka.ms/install-powershell.ps1 > $env:TEMP\\runme.ps1; C:\\Users\\WDAGUtilityAccount\\AppData\\Local\\Temp\\runme.ps1 -UseMSI -Quiet -AddExplorerContextMenu");
            _sbScoopPackageManagerBuilder.AppendLine(@"Write-Host Downloading VT-CLI");
            _sbScoopPackageManagerBuilder.AppendLine("iwr https://github.com/VirusTotal/vt-cli/releases/download/0.10.4/Windows64.zip -outfile C:\\Users\\WDAGUtilityAccount\\AppData\\Local\\Temp\\vt-cli.zip; Expand-Archive -Path C:\\Users\\WDAGUtilityAccount\\AppData\\Local\\Temp\\vt-cli.zip -DestinationPath C:\\Users\\WDAGUtilityAccount\\Desktop");
            _sbScoopPackageManagerBuilder.AppendLine("echo apikey = '<apikeyhere>' >> C:\\Users\\WDAGUtilityAccount\\.vt.toml");
            _sbScoopPackageManagerBuilder.AppendLine(@"Write-Host Start executing scoop package manager");
            _sbScoopPackageManagerBuilder.AppendLine("irm https://get.scoop.sh > $env:TEMP\\install.ps1; C:\\Users\\WDAGUtilityAccount\\AppData\\Local\\Temp\\install.ps1 -RunAsAdmin");
            _sbScoopPackageManagerBuilder.AppendLine("scoop install git");
            _sbScoopPackageManagerBuilder.AppendLine("scoop bucket add xkyii https://github.com/xkyii/scoop-xkyii.git");
            _sbScoopPackageManagerBuilder.AppendLine("scoop bucket add extras");
            _sbScoopPackageManagerBuilder.AppendLine("scoop bucket add nerd-fonts");
            _sbScoopPackageManagerBuilder.AppendLine("scoop bucket add nirsoft");
            _sbScoopPackageManagerBuilder.AppendLine("scoop bucket add java");
            _sbScoopPackageManagerBuilder.AppendLine("scoop bucket add jetbrains");
            _sbScoopPackageManagerBuilder.AppendLine("scoop bucket add nonportable");
            _sbScoopPackageManagerBuilder.AppendLine("scoop bucket add php");

            _sbScoopPackageManagerBuilder.Append("scoop install").Append(" ").AppendLine(string.Join(" ", Applications));
            _sbScoopPackageManagerBuilder.AppendLine(@"Write-Host Finished executing scoop package manager");
            _sbScoopPackageManagerBuilder.AppendLine(@"Write-Host Copying shortcuts");
            _sbScoopPackageManagerBuilder.AppendLine("copy-item -path 'C:\\Users\\WDAGUtilityAccount\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Scoop Apps\\*' -destination  C:\\Users\\WDAGUtilityAccount\\Desktop -Recurse -force");
            _sbScoopPackageManagerBuilder.AppendLine(@"Write-Host Installing Office");
            _sbScoopPackageManagerBuilder.AppendLine("C:\\Users\\WDAGUtilityAccount\\Desktop\\setups\\setup.exe /configure C:\\Users\\WDAGUtilityAccount\\Desktop\\setups\\Config.xml");
            _sbScoopPackageManagerBuilder.AppendLine(@"Write-Host Done!");
            _sbScoopPackageManagerBuilder.AppendLine(@"Write-Host Dont forget to install npcap using 'C:\Users\WDAGUtilityAccount\scoop\apps\wireshark\current\npcap-installer.exe' and vcredist if needed");
            string fullScriptPath = Path.Combine(options.RootFilesDirectoryLocation, _packageManagerScriptName);
            byte[] content = new UTF8Encoding().GetBytes(_sbScoopPackageManagerBuilder.ToString());

            await _writeAllBytesAsync(fullScriptPath, content, cancellationToken);

            ScriptFile = new FileInfo(fullScriptPath);
            
        }
    }
}
