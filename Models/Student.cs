namespace AdminManagement
{
    public class Student
    {
        public int STT { get; set;}
        public int ID { get; set; }
        public string? NameStudent { get; set; }
        public string? Class { get; set; }
        public string? Address { get; set; }
        public string? Parents { get; set; }
    }
    public class ClassStudent
    {
        public string? Class { get; set; }
    }
}