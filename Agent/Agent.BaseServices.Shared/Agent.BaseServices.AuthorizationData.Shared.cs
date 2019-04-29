using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Agent.BaseServices {
    public interface IAuthorizationData {
        string Placeholder { get; set; }
    }

    public class AuthorizationData : IAuthorizationData {
        public AuthorizationData() : this(string.Empty) { }

        public AuthorizationData(string placeholder) {
            Placeholder=placeholder??throw new ArgumentNullException(nameof(placeholder));
        }

        public string Placeholder { get; set; }
    }
}
