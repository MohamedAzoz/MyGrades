namespace MyGrades.Domain.Entities
{
    public class StudentSubject
    {
        public int Id { get; set; }
        // مفتاح خارجي للطالب
        public int StudentId { get; set; }
        public Student Student { get; set; }
        // مفتاح خارجي للمادة
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public Grade? Grade { get; set; }
    }

}
