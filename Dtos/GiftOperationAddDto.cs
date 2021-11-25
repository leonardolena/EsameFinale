namespace SantaClausCrm.Dtos
{
    public class GiftOperationAddDto : Dto
    {
        public int GiftId { get; set; }
        public int OperationId { get; set; }
        public int ElfId { get; set; }
        private int _uncleId = -1;
        public int UncleChristmasId { 
            get { return _uncleId; }
            set { _uncleId = value;}}
    }
}
