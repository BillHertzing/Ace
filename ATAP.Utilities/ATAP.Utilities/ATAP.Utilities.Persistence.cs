using System;
using System.Collections.Generic;
using System.Threading;

namespace ATAP.Utilities.Persistence {
    //ToDo: Add a CancellationToken
    public abstract class PersistenceSetupInitializationData {
        public CancellationToken CancellationToken;
    }

    public class PersistenceViaDBSetupInitializationData : PersistenceSetupInitializationData {
        public string DBConnectionString;
    }

    public abstract class PersistenceSetupResults {
        public bool Success;
    }

    //ToDo: Add a CancellationToken
    public abstract class PersistenceInsertData {
        public PersistenceSetupResults PersistenceSetupResults;
        public List<string> DList;
    }
    public abstract class PersistenceInsertResults {
        public bool Success;
    }

    // ToDo Make writeFileInfo into a thread-safe structure
    //ToDo: Add a CancellationToken
    public abstract class PersistenceTearDownInitializationData {
        public PersistenceSetupResults PersistenceSetupResults;
    }

    public abstract class PersistenceTearDownResults {
        public bool Success;
    }

    public abstract class PersistenceAbstract {
        public PersistenceSetupInitializationData PersistenceSetupInitializationData;
        public PersistenceSetupResults PersistenceSetupResults;
        public Func<PersistenceSetupInitializationData, PersistenceSetupResults> PersistenceSetup;
        public PersistenceInsertData PersistenceInsertData;
        public PersistenceInsertResults PersistenceInsertResults;
        public Func<PersistenceInsertData, PersistenceSetupResults, PersistenceInsertResults> PersistenceInsert;
        public PersistenceTearDownInitializationData PersistenceTearDownInitializationData;
        public PersistenceTearDownResults PersistenceTearDownResults;
        public Func<PersistenceTearDownInitializationData, PersistenceSetupResults, PersistenceTearDownResults> PersistenceTearDown;
    }

}
