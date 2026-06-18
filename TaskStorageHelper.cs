using System;
using System.Collections.Generic;
using System.Linq;

namespace AiAssistanceUI
{
    public class TaskStorageHelper
    {
        private readonly ApplicationDbContext _db;

        public TaskStorageHelper()
        {
            _db = new ApplicationDbContext();

            
            _db.Database.EnsureCreated();
        }

        public List<TaskItem> LoadTasks()
        {
            try
            {
                return _db.Tasks.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading tasks from database: {ex.Message}");
            }
        }

        public void AddTask(string title, string description, string reminder)
        {
            try
            {
                var task = new TaskItem
                {
                    Title = title,
                    Description = description ?? "",
                    Reminder = reminder ?? "",
                    IsComplete = false,
                    CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                };

                _db.Tasks.Add(task);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding task to database: {ex.Message}");
            }
        }

        public void MarkAsComplete(int id)
        {
            try
            {
                var task = _db.Tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    task.IsComplete = true;
                    _db.Tasks.Update(task);
                    _db.SaveChanges();
                }
                else
                {
                    throw new Exception($"Task with ID {id} not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error marking task as complete: {ex.Message}");
            }
        }

        public void DeleteTask(int id)
        {
            try
            {
                var task = _db.Tasks.FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    _db.Tasks.Remove(task);
                    _db.SaveChanges();
                }
                else
                {
                    throw new Exception($"Task with ID {id} not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting task: {ex.Message}");
            }
        }
    }
}