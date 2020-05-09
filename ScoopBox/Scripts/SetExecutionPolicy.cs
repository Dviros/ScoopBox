﻿using ScoopBox.Scripts.Abstract;
using System.Text;

namespace ScoopBox.Scripts
{
    public class SetExecutionPolicy : ISetExecutionPolicy
    {
        public string Set()
        {
            StringBuilder executionPolicy = new StringBuilder();

            executionPolicy.Append("Set-ExecutionPolicy ");
            executionPolicy.Append("Bypass ");
            executionPolicy.Append("-Force");

            executionPolicy.AppendLine();
            return executionPolicy.ToString();
        }
    }
}
