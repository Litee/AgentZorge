using JetBrains.ReSharper.Daemon;

namespace AgentZorge.DaemonStage.Highlights
{
    [StaticSeverityHighlighting(Severity.ERROR, "CSharpInfo")]
    public class AgentZorgeHighlighting : IHighlighting
    {
        private string _tooltip;

        public AgentZorgeHighlighting(string tooltip)
        {
            _tooltip = tooltip;
        }

        public string ErrorStripeToolTip
        {
            get { return _tooltip; }
        }

        public bool IsValid()
        {
            return true;
        }

        public int NavigationOffsetPatch
        {
            get { return 0; }
        }

        public string ToolTip
        {
            get { return _tooltip; }
        }
    }
}
