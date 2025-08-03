using System;
using System.Threading.Tasks;
using GTour.Abstractions.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GTour.Interops {
    public class JsInteropPopper : IJsInteropPopper, IAsyncDisposable {
        #region ctor

        public JsInteropPopper(IJSRuntime jsRuntime) {
            _moduleTask = new Lazy<Task<IJSObjectReference>>(() =>
                jsRuntime.InvokeAsync<IJSObjectReference>("import", _basePath).AsTask());
        }

        #endregion

        #region Dispose

        public async ValueTask DisposeAsync() {
            if (_moduleTask.IsValueCreated) {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
        }

        #endregion

        #region Members

        private const string _basePath = "./_content/STGTour.GTour/js/JsInteropPopper.min.js";
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

        #endregion

        #region Methods

        public async ValueTask<string> SetTourStepPopperByElement(ElementReference forElement,
            ElementReference gtourWrapper, string placement, string strategy) {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<string>("SetTourStepPopperByElement", forElement, gtourWrapper, placement,
                strategy);
        }

        public async ValueTask<string> SetTourStepPopperBySelector(string forElementSelector,
            ElementReference gtourWrapper, string placement, string strategy) {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<string>("SetTourStepPopperBySelector", forElementSelector, gtourWrapper,
                placement, strategy);
        }

        public async ValueTask<string> ResetTourStepPopper(ElementReference gtourWrapper, string placement,
            string strategy) {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<string>("ResetTourStepPopper", gtourWrapper, placement, strategy);
        }

        #endregion
    }
}
