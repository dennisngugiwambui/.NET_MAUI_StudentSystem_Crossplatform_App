using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.IO;
using SQLite;
using static StudentSystem.Term;
using static StudentSystem.Course;
using static StudentSystem.Assesment;

namespace StudentSystem.DataBase
{
    public class DatabaseHelper
    {
        SQLiteAsyncConnection Database;

        public DatabaseHelper()
        {
        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<TermRecord>();
            var records = await Database.CreateTableAsync<CourseRecord>();
            var assesment = await Database.CreateTableAsync<AssessmentRecord>();
        }


        //Getting the items

        public async Task<List<TermRecord>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<TermRecord>().ToListAsync();
        }

        public async Task<List<CourseRecord>> GetRecordAsync()
        {
            await Init();
            return await Database.Table<CourseRecord>().ToListAsync();
        }
        public async Task<List<AssessmentRecord>> GetAssessmentsAsync()
        {
            await Init();
            return await Database.Table<AssessmentRecord>().ToListAsync();
        }


        //Getting the Items

        public async Task<TermRecord> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<TermRecord>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<CourseRecord> GetRecordAsync(int id)
        {
            await Init();
            return await Database.Table<CourseRecord>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public async Task<AssessmentRecord> GetAssessmentAsync(int id)
        {
            await Init();
            return await Database.Table<AssessmentRecord>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }


        //Saving the items now
        public async Task<int> SaveItemAsync(TermRecord item)
        {
            await Init();
            if (item.ID != 0)
                return await Database.UpdateAsync(item);
            else
                return await Database.InsertAsync(item);
        }

        public async Task<int> SaveRecordAsync(CourseRecord courseRecord)
        {
            await Init();
            if (courseRecord.ID != 0)
                return await Database.UpdateAsync(courseRecord);
            else
                return await Database.InsertAsync(courseRecord);
        }

        public async Task<int> SaveAssessmentAsync(AssessmentRecord assessmentRecord)
        {
            await Init();
            if (assessmentRecord.ID != 0)
                return await Database.UpdateAsync(assessmentRecord);
            else
                return await Database.InsertAsync(assessmentRecord);
        }

        //Deleting the items

        public async Task<int> DeleteItemAsync(TermRecord item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }

        public async Task<int> DeleteRecordAsync(CourseRecord courseRecord)
        {
            await Init();
            return await Database.DeleteAsync(courseRecord);
        }

        public async Task<int> DeleteAssessmentAsync(AssessmentRecord assessmentRecord)
        {
            await Init();
            return await Database.DeleteAsync(assessmentRecord);
        }

        //files

        public async Task<int> SaveFileAsync(FileRecord fileRecord)
        {
            await Init();
            if (fileRecord.ID != 0)
                return await Database.UpdateAsync(fileRecord);
            else
                return await Database.InsertAsync(fileRecord);
        }

        // Getting files
        public async Task<List<FileRecord>> GetFilesAsync()
        {
            await Init();
            return await Database.Table<FileRecord>().ToListAsync();
        }

        public async Task<FileRecord> GetFileAsync(int id)
        {
            await Init();
            return await Database.Table<FileRecord>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        // Deleting files
        public async Task<int> DeleteFileAsync(FileRecord fileRecord)
        {
            await Init();
            return await Database.DeleteAsync(fileRecord);
        }
    }
}
