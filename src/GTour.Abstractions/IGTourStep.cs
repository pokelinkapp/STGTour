using System.Threading.Tasks;

namespace GTour.Abstractions {
    public interface IGTourStep {
        string StepName { get; set; }

        int? TourStepSequence { get; set; }

        bool SkipStep { get; set; }

        Task CancelTour();

        Task PreviousStep();

        Task NextStep();

        Task GoToStep(string stepName);

        Task CompleteTour();

        Task Initialise();

        Task DeActivate();

        Task Activate(bool isFirstStep, bool isLastStep);
    }
}
