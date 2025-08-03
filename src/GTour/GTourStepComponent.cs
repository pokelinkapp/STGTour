using System;
using System.Threading.Tasks;
using GTour.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace GTour {
    public abstract class GTourStepComponent : ComponentBase, IGTourStep, IDisposable {
        #region Properties

        [Inject]
        public IGTourService GTourService { get; set; }

        [Inject]
        protected ILogger<GTourStepComponent> Logger { get; set; }

        /// <summary>
        ///     Gets or sets the Unique step name
        /// </summary>
        [Parameter]
        public string StepName { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        ///     Gets or sets the tour step sequence
        /// </summary>
        [Parameter]
        public int? TourStepSequence { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating if the step should be skipped
        /// </summary>
        [Parameter]
        public bool SkipStep { get; set; }

        public bool IsFirstStep { get; private set; }

        public bool IsLastStep { get; private set; }

        public bool IsActiveStep { get; private set; }

        #endregion

        #region Event Callbacks

        [Parameter]
        public EventCallback<IGTourStep> OnStepActivated { get; set; }

        [Parameter]
        public EventCallback<IGTourStep> OnStepDeActivated { get; set; }

        [Parameter]
        public EventCallback<IGTourStep> OnTourCanceled { get; set; }

        [Parameter]
        public EventCallback<IGTourStep> OnTourCompleted { get; set; }

        [Parameter]
        public EventCallback<IGTourStep> OnNavigatePrevious { get; set; }

        [Parameter]
        public EventCallback<IGTourStep> OnNavigateNext { get; set; }

        #endregion

        #region Methods

        public async Task CancelTour() {
            await OnTourCanceled.InvokeAsync(this);
            await GTourService?.CancelTour();
        }

        public async Task PreviousStep() {
            await OnNavigatePrevious.InvokeAsync(this);
            await GTourService?.PreviousStep();
        }

        public async Task NextStep() {
            await OnNavigateNext.InvokeAsync(this);
            await GTourService?.NextStep();
        }

        public async Task GoToStep(string stepName) {
            await GTourService?.GoToStep(stepName);
        }

        public async Task CompleteTour() {
            await OnTourCompleted.InvokeAsync(this);
            await GTourService?.CompleteTour();
        }

        public Task Initialise() {
            IsActiveStep = false;
            IsFirstStep = false;
            IsLastStep = false;

            StateHasChanged();

            return Task.CompletedTask;
        }

        protected virtual Task RunActivation() {
            return Task.CompletedTask;
        }

        protected virtual Task RunDeActivation() {
            return Task.CompletedTask;
        }

        public async Task DeActivate() {
            IsActiveStep = false;

            await RunDeActivation();

            await OnStepDeActivated.InvokeAsync(this);

            StateHasChanged();
        }

        public async Task Activate(bool isFirstStep, bool isLastStep) {
            IsActiveStep = true;
            IsFirstStep = isFirstStep;
            IsLastStep = isLastStep;

            await RunActivation();

            await OnStepActivated.InvokeAsync(this);

            StateHasChanged();
        }

        #endregion

        #region Dispose

        public void Dispose() {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing) { }

        #endregion
    }
}
