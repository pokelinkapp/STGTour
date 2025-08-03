using System.Collections.Generic;
using System.Threading.Tasks;
using GTour.Abstractions;
using GTour.Abstractions.Common;
using GTour.Abstractions.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace GTour.Components {
    public partial class GuidedTour : GTourComponent {
        #region Properties

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> UnmatchedAttributes { get; set; }

        [Inject]
        internal IJsInteropPopper JsInteropPopper { get; set; }

        [Inject]
        internal IJsInteropCommon JsInteropCommon { get; set; }

        internal ElementReference OverlayElement { get; set; }

        internal ElementReference WrapperElement { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string OverlayClass { get; set; }

        [Parameter]
        public string ArrowClass { get; set; }

        [Parameter]
        public RenderFragment ArrowContent { get; set; }

        [Parameter]
        public bool CloseOnOverlayClick { get; set; }

        [Parameter]
        public bool OverlayEnabled { get; set; } = true;

        [Parameter]
        public bool ShowArrow { get; set; } = true;

        [Parameter]
        public string TourWrapperClass { get; set; }

        [Parameter]
        public string HighlightClass { get; set; } = "element-highlight";

        [Parameter]
        public bool HighlightEnabled { get; set; } = true;

        [Parameter]
        public bool ScrollToElement { get; set; }

        public ITheme SelectedTheme => GTourService.Theme;

        #endregion

        #region Methods

        internal void StepUnRegistered(GuidedTourStep guidedTourStep) {
            if (Steps != null) {
                RemoveGTourStep(guidedTourStep);
            }
        }

        internal void StepRegistered(GuidedTourStep guidedTourStep) {
            if (Steps != null) {
                AddGTourStep(guidedTourStep);
            }
        }

        internal async Task JsInteropStart(GuidedTourStep guidedTourStep) {
            if (guidedTourStep == null) {
                Logger?.LogWarning($"{nameof(JsInteropStart)}: Guided tour step is null");
                return;
            }

            if (!string.IsNullOrEmpty(guidedTourStep.ElementSelector)) {
                if (ScrollToElement) {
                    await JsInteropCommon.ScrollToElement(guidedTourStep.ElementSelector);
                }

                await JsInteropPopper.SetTourStepPopperBySelector(guidedTourStep.ElementSelector, WrapperElement,
                    guidedTourStep.PopupPlacement.GetEnumDisplay(), guidedTourStep.PopupStrategy.GetEnumDisplay());

                if (HighlightEnabled && !string.IsNullOrEmpty(HighlightClass)) {
                    await JsInteropCommon.AddClassToElement(guidedTourStep.ElementSelector, HighlightClass);
                }
            } else {
                await JsInteropPopper.ResetTourStepPopper(WrapperElement,
                    guidedTourStep.PopupPlacement.GetEnumDisplay(), guidedTourStep.PopupStrategy.GetEnumDisplay());
            }
        }

        internal async Task JsInteropEnd(GuidedTourStep guidedTourStep) {
            if (guidedTourStep == null) {
                Logger?.LogWarning($"{nameof(JsInteropEnd)}: Guided tour step is null");
                return;
            }

            if (HighlightEnabled && !string.IsNullOrEmpty(HighlightClass)) {
                await JsInteropCommon.RemoveClassFromElement(guidedTourStep.ElementSelector, HighlightClass);
            }
        }

        protected override async Task CleanupTour() {
            if (CurrentStep != null) {
                await JsInteropEnd(CurrentStep as GuidedTourStep);
            }
        }

        #endregion
    }
}
