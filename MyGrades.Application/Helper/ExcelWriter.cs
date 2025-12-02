using ClosedXML.Excel;
using MyGrades.Application.Contracts.DTOs.Grade;
using MyGrades.Application.Contracts.DTOs.User.Student;

namespace MyGrades.Application.Helper
{
    public class ExcelWriter
    {
        public MemoryStream WriteGradesToStream(List<GradeExcelWriterDto> grades)
        {
            var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Grades");
                // إضافة رؤوس الأعمدة
                worksheet.Cell("A1").Value = "Student Name";
                worksheet.Cell("B1").Value = "Subject";
                worksheet.Cell("C1").Value = "Grade";
                // إضافة البيانات

                worksheet.Rows().AdjustToContents();
                worksheet.Columns().AdjustToContents();
                worksheet.Cell("A2").InsertData(grades.Select(g => new { g.StudentName, g.SubjectName, g.GradeValue }));

                workbook.SaveAs(stream);
                return stream;
            }
        }

        public MemoryStream WriteGradesTemplateToStream()
        {
            var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("GradesTemplate");
                // إضافة رؤوس الأعمدة
                worksheet.Cell("A1").Value = "StudentId";
                worksheet.Cell("B1").Value = "Attendance";
                worksheet.Cell("C1").Value = "Tasks";
                worksheet.Cell("D1").Value = "Practical";
                worksheet.Cell("E1").Value = "SubjectName";
                worksheet.Rows().AdjustToContents();
                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(stream);
                return stream;
            }
        }
        public MemoryStream WriteAssistantsTemplateToStream()
        {
            var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("AssistantsTemplate");
                // إضافة رؤوس الأعمدة
                worksheet.Cell("A1").Value = "FullName";
                worksheet.Cell("B1").Value = "NationalId";
                worksheet.Cell("C1").Value = "DepartmentId";
                worksheet.Columns().AdjustToContents();
                //worksheet.Columns().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Rows().AdjustToContents();
                workbook.SaveAs(stream);
                return stream;
            }
        }

        public MemoryStream WriteStudentsTemplateToStream(List<UserExcelDto> students)
        {
            var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("StudentsTemplate");
                // إضافة رؤوس الأعمدة
                worksheet.Cell("A1").Value = "FullName";
                worksheet.Cell("B1").Value = "NationalId";
                worksheet.Cell("C1").Value = "Attendance";
                worksheet.Cell("D1").Value = "Tasks";
                worksheet.Cell("E1").Value = "Practical";
                worksheet.Rows().AdjustToContents();
                worksheet.Columns().AdjustToContents();
                worksheet.Cell("A2").InsertData(students.Select(s => new { s.FullName, s.NationalId }));
                workbook.SaveAs(stream);
                stream.Position = 0;
                return stream;
            }
        }

    }
}
