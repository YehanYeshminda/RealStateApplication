namespace API.Repos.Interfaces
{
    public interface IChartInterface
    {
        int GetTotalCallsLeft(int assignedUser);
        int GetTotalCallsLeftAdmin();

        int GetCallsLeftUser(int assignedUser);
        int GetCallsLeftUserAdmin();
        int GetLeadsConversionsTodayUser(int assignedUser);
        int GetLeadsConversionsTodayAdmin();
        int GetControlCallsMonthlyTargetAccordingToStaff(int staffId);

        int GeControltMonthlyCallsTargetAccordingToStaff(int staffId);
    }
}