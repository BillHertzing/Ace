using MethodBoundaryAspect.Fody.Attributes;

namespace Ace.GUI
{

    public sealed class LogMethodBoundariesInBlazorAttribute : OnMethodBoundaryAspect {

        public override void OnEntry(MethodExecutionArgs args) {
            MethodBoundaryLogProvider.LogMethodEntry(args.Method.DeclaringType.FullName, args.Method.Name);
            // ToDo: Log arguments (make this controlled in the GUI's ConfigurationRoot? after Configuration is implemented in Blazor)
            //    foreach (var item in args.Method.GetParameters()) {
            //        BlazorLogProviderComponent.LogMethodArguments($"{item.Name}: {args.Arguments[item.Position]}");
            //    }
            //
        }

        //public override void OnExit(MethodExecutionArgs args, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") {
        public override void OnExit(MethodExecutionArgs args) {
            // ToDo: Log return value (make this controlled in the GUI's ConfigurationRoot? after Configuration is implemented in Blazor)
            MethodBoundaryLogProvider.LogMethodExit(args.Method.DeclaringType.FullName, args.Method.Name);
        }


        public override void OnException(MethodExecutionArgs args) {
            //ToDo: Add Error level or category to BlazorLogProvider
            MethodBoundaryLogProvider.LogMethodException(args.Method.DeclaringType.FullName, args.Exception.GetType().ToString(), args.Exception.Message);
        }
    }

}
