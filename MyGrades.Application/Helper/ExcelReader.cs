using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.DTOs.Grade;
using MyGrades.Application.Contracts.DTOs.User.Assistant;
using MyGrades.Application.Contracts.DTOs.User.Student;
using MyGrades.Domain.Entities;
using System.Collections.Generic;

namespace MyGrades.Application.Helper
{

    public class ExcelReader
    {
        public Result<List<UserExcelDto>> ReadUsersFromStream(Stream stream)
        {
            var users = new List<UserExcelDto>();

            try
            {
                using (var workbook = new XLWorkbook(stream))
                {
                    // 1. التعامل مع الصفحة الأولى فقط لتجنب الأخطاء في الصفحات الإضافية
                    var worksheet = workbook.Worksheet(1);

                    // 2. التحقق من عدد الصفوف
                    var rows = worksheet.RowsUsed();
                    if (rows.Count() > 10001)
                    {
                        return Result<List<UserExcelDto>>.Failure("The number of rows exceeds the allowed limit of 10,000.");
                    }

                    // تخطي صف العناوين (Skip 1)
                    foreach (var row in rows.Skip(1))
                    {
                        // 3. قراءة البيانات بمرونة (بدون شرط DataType الصارم)
                        var fullName = row.Cell(1).GetValue<string>().Trim();
                        var nationalId = row.Cell(2).GetValue<string>().Trim();

                        // تجاهل الصفوف الفارغة إن وجدت
                        if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(nationalId))
                            continue;

                        // التحقق من صحة البيانات
                        if (nationalId.Length != 14 || !long.TryParse(nationalId, out _))
                        {
                            // يمكنك هنا إما إرجاع خطأ وإيقاف العملية، أو تخطي هذا الصف وتسجيله في قائمة أخطاء
                            // سأقوم بالتخطي هنا
                            continue;
                        }


                        users.Add(new UserExcelDto
                        {
                            FullName = fullName,
                            NationalId = nationalId
                        });
                    }
                }

                if (users.Count == 0)
                    return Result<List<UserExcelDto>>.Failure("No valid Users found in the file.");

                return Result<List<UserExcelDto>>.Success(users);
            }
            catch (Exception ex)
            {
                return Result<List<UserExcelDto>>.Failure($"Error reading Excel file: {ex.Message}");
            }
        }
        public Result<List<AssistantExcelDto>> ReadAssistantFromStream(Stream stream)
        {
            var assistants = new List<AssistantExcelDto>();

            try
            {
                using (var workbook = new XLWorkbook(stream))
                {
                    // 1. التعامل مع الصفحة الأولى فقط لتجنب الأخطاء في الصفحات الإضافية
                    var worksheet = workbook.Worksheet(1);

                    // 2. التحقق من عدد الصفوف
                    var rows = worksheet.RowsUsed();
                    if (rows.Count() > 10001)
                    {
                        return Result<List<AssistantExcelDto>>.Failure("The number of rows exceeds the allowed limit of 10,000.");
                    }

                    // تخطي صف العناوين (Skip 1)
                    foreach (var row in rows.Skip(1))
                    {
                        // 3. قراءة البيانات بمرونة (بدون شرط DataType الصارم)
                        string fullName = row.Cell(1).GetValue<string>().Trim();
                        string nationalId = row.Cell(2).GetValue<string>().Trim();
                        int departmentId = 0;
                        if (!row.Cell(3).TryGetValue(out departmentId))
                        {
                            // إذا لم يتمكن من القراءة كرقم صحيح، نتخطى السطر
                            continue;
                        }

                        // تجاهل الصفوف الفارغة إن وجدت
                        if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(nationalId)
                            || departmentId == 0)
                            continue;

                        // التحقق من صحة البيانات
                        if (nationalId.Length != 14 || !long.TryParse(nationalId, out _))
                        {
                            // يمكنك هنا إما إرجاع خطأ وإيقاف العملية، أو تخطي هذا الصف وتسجيله في قائمة أخطاء
                            // سأقوم بالتخطي هنا
                            continue;
                        }


                        assistants.Add(new AssistantExcelDto
                        {
                            FullName = fullName,
                            NationalId = nationalId,
                            DepartmentId=departmentId
                        });
                    }
                }

                if (assistants.Count == 0)
                    return Result<List<AssistantExcelDto>>.Failure("No valid Users found in the file.");

                return Result<List<AssistantExcelDto>>.Success(assistants);
            }
            catch (Exception ex)
            {
                return Result<List<AssistantExcelDto>>.Failure($"Error reading Excel file: {ex.Message}");
            }
        }

        public Result<List<GradeExcelDto>> ReadGradesFromStream(Stream stream)
        {
            var grades = new List<GradeExcelDto>();

            try
            {
                using (var workbook = new XLWorkbook(stream))
                {
                    // 1. التعامل مع الصفحة الأولى فقط لتجنب الأخطاء في الصفحات الإضافية
                    var worksheet = workbook.Worksheet(1);

                    // 2. التحقق من عدد الصفوف
                    var rows = worksheet.RowsUsed();
                    if (rows.Count() > 10001)
                    {
                        return Result<List<GradeExcelDto>>.Failure("The number of rows exceeds the allowed limit of 10,000.");
                    }

                    // تخطي صف العناوين (Skip 1)
                    foreach (var row in rows.Skip(1))
                    {
                        // 3. قراءة البيانات بمرونة (بدون شرط DataType الصارم)
                        int studentId = 0;
                        if (!row.Cell("F").TryGetValue(out studentId))
                        {
                            continue;
                        }
                        double attendance = row.Cell("B").GetValue<double>();
                        double tasks = row.Cell("C").GetValue<double>();
                        double practical = row.Cell("D").GetValue<double>();
                        // تجاهل الصفوف الفارغة إن وجدت
                        if (studentId == 0 || attendance == 0 || tasks == 0 || practical == 0)
                            continue;

                        grades.Add(new GradeExcelDto
                        {
                            StudentId = studentId,
                            Attendance = attendance,
                            Tasks = tasks,
                            Practical = practical
                        });
                    }
                }

                if (grades.Count == 0)
                    return Result<List<GradeExcelDto>>.Failure("No valid Grades found in the file.");

                return Result<List<GradeExcelDto>>.Success(grades);
            }
            catch (Exception ex)
            {
                return Result<List<GradeExcelDto>>.Failure($"Error reading Excel file: {ex.Message}");
            }
        }
      
    }
}
