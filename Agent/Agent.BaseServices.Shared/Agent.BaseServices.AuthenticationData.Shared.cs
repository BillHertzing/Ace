using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Agent.BaseServices
{
    public interface IAuthenticationData {
        string Placeholder { get; set; }
    }
    public class AuthenticationData : IAuthenticationData {
        public AuthenticationData() : this(string.Empty) { }

        public AuthenticationData(string placeholder) {
            Placeholder=placeholder??throw new ArgumentNullException(nameof(placeholder));
        }

        public string Placeholder { get; set; }

    }
}
