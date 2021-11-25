namespace SantaClausCrm.Dtos
{
    public class GiftOperationAddDto : Dto
    {
        public int GiftId { get; set; }
        public int OperationId { get; set; }
        public int ElfId { get; set; }
        public int UncleChristmasId { get; set; } = -1;
    }
}
