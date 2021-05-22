namespace DataAccessLibrary.Models.QneModels
{
    public class FGMASTERFILEMODEL
    {
        public string StockCode { get; set; }
        public string StockName { get; set; }
        public string CATEGORY { get; set; }
        public bool? SWINE { get; set; }
        public bool? POULTRY { get; set; }
        public bool? COMMON { get; set; }
        public int? CATID { get; set; }

    }
}
