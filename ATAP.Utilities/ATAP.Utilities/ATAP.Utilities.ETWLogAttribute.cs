using MethodBoundaryAspect.Fody.Attributes;

namespace ATAP.Utilities.ETW {
    public sealed class ETWLogAttribute : OnMethodBoundaryAspect {
        // public override void OnEntry(MethodExecutionArgs args, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") {
        public override void OnEntry(MethodExecutionArgs args) {
            ATAPUtilitiesETWProvider.Log.MethodBoundryFromAspect($"<{args.Method.DeclaringType.FullName}.{args.Method.Name}");
            //foreach (var item in args.Method.GetParameters()) {
            //    ATAPUtilitiesETWProvider.Log.Information($"{item.Name}: {args.Arguments[item.Position]}");
            //}
        }

        //public override void OnExit(MethodExecutionArgs args, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") {
        public override void OnExit(MethodExecutionArgs args) {
            // ATAPUtilitiesETWProvider.Log.MethodBoundry($">[{args.ReturnValue}]");
            ATAPUtilitiesETWProvider.Log.MethodBoundryFromAspect($">{args.Method.DeclaringType.FullName}.{args.Method.Name}");
        }

        public override void OnException(MethodExecutionArgs args) {
            //ToDo: Add Error level or category to ATAPUtilitiesETWProvider
            ATAPUtilitiesETWProvider.Log.Information($"OnException: {args.Exception.GetType()}: {args.Exception.Message}");
        }
    }
}
