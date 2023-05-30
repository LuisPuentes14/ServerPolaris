namespace ServerPolaris.Models.ViewModels
{
    public class VMIndex
    {
        public string DBName { get; set; }
        public string DatabaseShemaTable { get; set; }
        public string EqualityColumns { get; set; }
        public string inequalityColumns { get; set; }
        public string IncludedColums { get; set; }
        public string AvgUserImpact { get; set; }
        public string CreateCmd { get; set; }
    }
}
