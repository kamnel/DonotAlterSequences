
using Microsoft.SqlServer.Dac.Deployment;
using Microsoft.SqlServer.Dac.Extensibility;
using System;
using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DonotAlterSequences
{
    [ExportDeploymentPlanModifier("DonotAlterSequences", "1.0.0.0")]
    public class DeploymentFilter : DeploymentPlanModifier
    {

        protected override void OnExecute(DeploymentPlanContributorContext context)
        {
            try
            {
                PublishMessage(new ExtensibilityError("DonotAlterSequences DeploymentFilter", Severity.Message));

                var nextStep = context.PlanHandle.Head;
                while (nextStep != null)
                {
                    var step = nextStep;
                    nextStep = step.Next;

                    if (step is AlterElementStep alterStep)
                    {
                        var objectName = alterStep.TargetElement?.Name?.ToString() ?? "";
                        bool shouldRemoveFromPlan = false;


                        if (alterStep != null)
                        {
                            var script = (TSqlScript)alterStep.Script;
                            var batch = script?.Batches.FirstOrDefault();
                            if (batch != null)
                            {
                                //is there a alter statement for our sequence if so abort the whole thing
                                var statement = batch.Statements.FirstOrDefault();
                                if (statement is AlterSequenceStatement alterSequenceStatement)
                                {
                                    shouldRemoveFromPlan = true;
                                }

                                PublishMessage(new ExtensibilityError($"{batch.Statements.Count}", Severity.Message));
                            }
                        }

                        if (shouldRemoveFromPlan)
                        {
                            Remove(context.PlanHandle, step);
                            PublishMessage(new ExtensibilityError($"Step removed from deployment by SqlPackageFilter, object: {objectName}", Severity.Message));
                            PublishMessage(new ExtensibilityError($"{string.Concat(step.GenerateTSQL())}", Severity.Message));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //global exception as we don't want to break sqlpackage.exe
                PublishMessage(new ExtensibilityError($"Error in DeploymentFilter: {e.Message}\r\nStack: {e.StackTrace}", Severity.Error));
            }
        }
    }
}
