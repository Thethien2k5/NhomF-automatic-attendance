namespace DTO
{
    public class Student
    {
        public int STT { get; set;}
        public int ID { get; set; }
        public string? NameStudent { get; set; }
        public string? Class { get; set; }
        public string? Parents { get; set; }
        public string? ParentEmail{ get; set; }
    }
    public class ClassStudent
    {
        public string? Class { get; set; }
    }
}