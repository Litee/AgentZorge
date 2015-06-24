using JetBrains.DocumentModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;

namespace AgentZorge.DaemonStage.Highlights
{
    [StaticSeverityHighlighting(Severity.ERROR, "CSharpInfo")]
    public class AgentZorgeHighlighting : IHighlighting
    {
        private readonly IExpression _expression;
        private readonly string _tooltip;

        public AgentZorgeHighlighting(IExpression expression, string tooltip)
        {
            _expression = expression;
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

        public DocumentRange CalculateRange()
        {
            return _expression.GetDocumentRange();
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
