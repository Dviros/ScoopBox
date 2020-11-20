﻿using ScoopBox.Scripts.PackageManagers;
using ScoopBox.Scripts.PackageManagers.Scoop;
using System.Collections.Generic;
using Xunit;

namespace ScoopBox.Test.PackageManager.IPackageManagerTests
{
    public class IsAssignablePropertyTests
    {
        [Fact]
        public void IsOptionsAssignableToIOptions()
        {
            IPackageManager packageManager = new ScoopPackageManager(new List<string>() { "git" });

            Assert.IsAssignableFrom<IPackageManager>(packageManager);
        }
    }
}
