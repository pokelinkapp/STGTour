using System;
using System.Threading.Tasks;
using GTour.Abstractions.JsInterop;
using Microsoft.JSInterop;

namespace GTour.Interops {
    public class JsInteropCommon : IJsInteropCommon, IAsyncDisposable {
        #region ctor

        public JsInteropCommon(IJSRuntime jsRuntime) {
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

        private const string _basePath = "./_content/STGTour.GTour/js/JsInteropCommon.min.js";
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

        #endregion

        #region Methods

        public async ValueTask<string> ScrollToElement(string elementSelector) {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<string>("ScrollToElement", elementSelector);
        }

        public async ValueTask<string> AddClassToElement(string elementSelector, string className) {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<string>("AddClassToElement", elementSelector, className);
        }

        public async ValueTask<string> RemoveClassFromElement(string elementSelector, string className) {
            var module = await _moduleTask.Value;
            return await module.InvokeAsync<string>("RemoveClassFromElement", elementSelector, className);
        }

        #endregion
    }
}
