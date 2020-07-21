using Searchfight.Domain.Interfaces;
using System;
using System.Linq;

namespace Searchfight.Domain.Validators
{
    /// <summary>
    /// Validates input args
    /// </summary>
    public class InputValidator : IInputValidator
    {
        public void Validate(string[] searchTerms)
        {
            if (!searchTerms.Any())
            {
                throw new Exception("No parameters specified. Please provide at least one search term.");
            }
        }
    }
}
