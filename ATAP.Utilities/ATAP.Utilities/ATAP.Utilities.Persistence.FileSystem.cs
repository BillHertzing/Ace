using System;
using System.IO;

namespace ATAP.Utilities.Persistence.FileSystem {
    public class PersistenceViaFileSetupInitializationData : PersistenceSetupInitializationData {
        public string FilePath;
    }

    public class PersistenceViaFileSetupResults : PersistenceSetupResults {
        public FileStream FileStream;
        public StreamWriter StreamWriter;
    }

    public class PersistenceViaFileInsertData : PersistenceInsertData {

    }

    public class PersistenceViaFileInsertResults : PersistenceInsertResults {
    }

    public class PersistenceViaFileTearDownInitializationData : PersistenceTearDownInitializationData {
    }

    public class PersistenceViaFileTearDownResults : PersistenceTearDownResults {
    }

    public class PersistenceViaFile : PersistenceAbstract {

        public Func<PersistenceViaFileSetupInitializationData, PersistenceViaFileSetupResults> PersistenceViaFileSetup;
        public Func<PersistenceViaFileInsertData, PersistenceViaFileSetupResults, PersistenceViaFileInsertResults> PersistenceViaFileViaFileInsert;
        public Func<PersistenceViaFileTearDownInitializationData, PersistenceViaFileTearDownResults> PersistenceViaFileTearDown;

    }
}
