﻿using Xunit;

namespace ScoopBox.Test.SandboxConfigurations.ISandboxConfigurationBuilderTests
{
    public class IsAssignablePropertyTests
    {
        [Fact]
        public void IsISandboxConfigurationBuilderAssignableToSandboxConfigurationBuilder()
        {
            SandboxConfigurationBuilder sandboxConfigurationBuilder = new SandboxConfigurationBuilder(new Options());

            Assert.IsAssignableFrom<ISandboxConfigurationBuilder>(sandboxConfigurationBuilder);
        }
    }
}
