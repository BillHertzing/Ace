using ServiceStack.Configuration;
using System.Collections.Concurrent;


namespace ATAP.Utilities.ServiceStack {
    public class AppSettingsDictionary {
        private readonly ConcurrentDictionary<string, IAppSettings> _cache;
        public AppSettingsDictionary() {
            _cache=new ConcurrentDictionary<string, IAppSettings>();
        }
        public bool TryGetAppSettings(string appSettingKey, out IAppSettings appSettings) {
            IAppSettings _appSettings = new AppSettings();
            bool success = _cache.TryGetValue(appSettingKey, out _appSettings);
            if (success) {
                appSettings=_appSettings;
            } else {
                appSettings=new AppSettings();
            }
            return success;
        }
    }
}
