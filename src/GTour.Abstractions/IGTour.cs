namespace GTour.Abstractions {
    public interface IGTour {
        #region Properties

        string TourId { get; set; }

        bool IsOnLastStep { get; }

        bool IsOnFirstStep { get; }

        int? CurrentStepIndex { get; }

        string CurrentStepName { get; }

        #endregion
    }
}
