namespace SantaClausCrm.Models
{
    public class GiftOperation : BaseModel
    {
        public int GiftId { get; set; }
        public Gift Gift { get; set; }

        public int OperationId { get; set; }
        public Operation Operation { get; set; }

        public int ElfId { get; set; }
        public Elf Elf { get; set; }
    }
}
