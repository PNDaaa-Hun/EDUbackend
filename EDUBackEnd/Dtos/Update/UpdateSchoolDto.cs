namespace EDUBackEnd.Dtos.Update
{
    public class UpdateSchoolDto
    {
        public string? Name { get; set; }
        public required int ActiveStudents { get; set; }
    }
}
