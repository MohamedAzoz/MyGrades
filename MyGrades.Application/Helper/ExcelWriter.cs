using ClosedXML.Excel;
using MyGrades.Application.Contracts.DTOs.Grade;
using MyGrades.Application.Contracts.DTOs.User.Student;

namespace MyGrades.Application.Helper
{
    public class ExcelWriter : IExcelWriter
    {
        public MemoryStream WriteGradesToStream(List<GradeExcelWriterDto> grades)
        {
            var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Grades");
                // إضافة رؤوس الأعمدة
                worksheet.Cell("A1").Value = "Student Name";
                worksheet.Cell("B1").Value = "Attendance";
                worksheet.Cell("C1").Value = "Tasks";
                worksheet.Cell("D1").Value = "Practical";
                worksheet.Cell("E1").Value = "Total Score";
                // إضافة البيانات

                worksheet.Rows().AdjustToContents();
                worksheet.Columns().AdjustToContents();
                worksheet.Cell("A2").InsertData(grades.Select(g => new { g.StudentName, g.Attendance, g.Tasks, g.Practical, g.TotalScore }));

                workbook.SaveAs(stream);
                return stream;
            }
        }

        // 🚨 يجب تعديل الدالة لتقبل قائمة بالطلاب
        public MemoryStream WriteGradesTemplateToStream(List<UserExcelWriterDto> students)
        {
            var stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("GradesTemplate");

                // --- إضافة رؤوس الأعمدة المرئية للدكتور ---
                // العمود A: اسم الطالب (للعرض فقط)
                worksheet.Cell("A1").Value = "Full Name";
                // الأعمدة B, C, D: الدرجات المدخلة
                worksheet.Cell("B1").Value = "Attendance";
                worksheet.Cell("C1").Value = "Tasks";
                worksheet.Cell("D1").Value = "Practical";

                // --- إضافة الأعمدة المخفية (للنظام) --- 
                // العمود F: Student ID (مخفي - هذا هو الـ ID الذي سنعتمد عليه عند الرفع)
                worksheet.Cell("F1").Value = "StudentId (Hidden)";
                worksheet.Column("F").Hide();

                // --- ملء بيانات الطلاب والأعمدة المخفية ---
                int currentRow = 2;
                foreach (var student in students)
                {
                    // العمود A: ملء اسم الطالب (للعرض)
                    worksheet.Cell($"A{currentRow}").Value = student.FullName; 

                    // العمود F: ملء الـ ID الفعلي للطالب (مخفي - الأهم للقراءة)
                    // 🚨 سنفترض أن UserExcelWriterDto به خاصية Id
                    worksheet.Cell($"F{currentRow}").Value = student.Id;

                    // تعيين قيم افتراضية للدرجات (اختياري)
                    worksheet.Cell($"B{currentRow}").Value = 0;
                    worksheet.Cell($"C{currentRow}").Value = 0;
                    worksheet.Cell($"D{currentRow}").Value = 0;

                    currentRow++;
                }

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
