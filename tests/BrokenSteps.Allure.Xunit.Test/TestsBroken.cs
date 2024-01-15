using System;
using System.Threading.Tasks;
using Allure.Net.Commons;
using Allure.XUnit.Attributes.Steps;
using Xunit;

namespace BrokenSteps.Allure.Xunit.Test
{
    public class TestsBroken
    {
        async Task AsyncWaitMethod()
        {
            await Task.Delay(1000);
        }

        [AllureStep("Say hello")]
        private void TaggedSayHello()
        {
            Console.Write("Hello");
        }
        
        [AllureStep("Async wait with tag")]
        async Task TaggedAsyncWaitMethod()
        {
            await Task.Delay(1000);
        }

        private void SayHello()
        {
            Console.Write("Hello");
        }
        
        // Works fine
        [Fact]
        public async Task Test1()
        {
            AllureApi.Step("Nested method async", async () =>
            {
                await AsyncWaitMethod();
            });
        }

        // System.InvalidOperationException: No fixture, test, or step context is active.
        //
        // System.InvalidOperationException
        //            No fixture, test, or step context is active.
        //    at Allure.Net.Commons.AllureContext.get_CurrentStepContainer()
        //    at Allure.Net.Commons.AllureLifecycle.StartStep(StepResult result)
        //    at Allure.Net.Commons.ExtendedApi.StartStep(String name)
        //    at Allure.Net.Commons.AllureApi.ExecuteAction[T](String name, Action`1 start, Func`1 action, Action pass, Action fail)
        //    at Allure.Net.Commons.AllureApi.ExecuteStep[T](String name, Func`1 action)
        //    at Allure.Net.Commons.AllureApi.Step(String name, Action action)
        [Fact]
        public async Task Test2()
        {
            AllureApi.Step("Nested method not async in not async block", () =>
            {
                SayHello();
            });
        }

        // Works fine
        [Fact]
        public async Task Test3()
        {
            AllureApi.Step("Nested method not async in async block", async () =>
            {
                SayHello();
            });
        }
        
        // System.InvalidOperationException
        // No step context is active.
        //    at Allure.Net.Commons.AllureContext.get_CurrentStep()
        // at Allure.Net.Commons.AllureLifecycle.UpdateStep(Action`1 update)
        // at Allure.Net.Commons.ExtendedApi.BreakStep(Action`1 updateResults)
        // at Allure.Net.Commons.Steps.AllureAbstractStepAspect.ThrowStep(MethodBase metadata, Exception e)
        // at Allure.Net.Commons.Steps.AllureAbstractStepAspect.OnTargetInvokeException(MethodBase metadata, Exception e)
        // at Allure.Net.Commons.Steps.AllureAbstractStepAspect.WrapAsync(Func`2 target, Object[] args, MethodBase metadata, String stepName, List`1 stepParameters)
        //
        [Fact]
        public async Task Test4()
        {
            await TaggedAsyncWaitMethod();
        }
        
        // System.InvalidOperationException
        // No step context is active.
        //     at Allure.Net.Commons.AllureContext.get_CurrentStep()
        // at Allure.Net.Commons.AllureLifecycle.UpdateStep(Action`1 update)
        // at Allure.Net.Commons.ExtendedApi.BreakStep(Action`1 updateResults)
        // at Allure.Net.Commons.Steps.AllureAbstractStepAspect.ThrowStep(MethodBase metadata, Exception e)
        // at Allure.Net.Commons.Steps.AllureAbstractStepAspect.OnTargetInvokeException(MethodBase metadata, Exception e)
        // at Allure.Net.Commons.Steps.AllureAbstractStepAspect.WrapSyncVoid(Func`2 target, Object[] args, MethodBase metadata, String stepName, List`1 stepParameters)
        
        [Fact]
        public async Task Test5()
        {
            TaggedSayHello();
        }
    }
}