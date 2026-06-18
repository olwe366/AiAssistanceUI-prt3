using System;
using System.Collections.Generic;
using System.Linq;

namespace AiAssistanceUI
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;
        private ActivityLogger _logger;

        public TaskManager()
        {
            _storage = new TaskStorageHelper();
            _logger = new ActivityLogger();
        }

        public void AddTask(string title, string description, string reminder)
        {
            try
            {
                _storage.AddTask(title, description, reminder);
                _logger.Log($"Task added: '{title}' {(string.IsNullOrEmpty(reminder) ? "(No reminder)" : $"(Reminder: {reminder})")}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add task: {ex.Message}");
            }
        }

        public List<TaskItem> GetAllTasks()
        {
            try
            {
                return _storage.LoadTasks();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load tasks: {ex.Message}");
            }
        }

        public void MarkAsComplete(int id)
        {
            try
            {
                var task = _storage.LoadTasks().FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    _storage.MarkAsComplete(id);
                    _logger.Log($"Task marked complete: '{task.Title}'");
                }
                else
                {
                    throw new Exception($"Task with ID {id} not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to mark task as complete: {ex.Message}");
            }
        }

        public void DeleteTask(int id)
        {
            try
            {
                var task = _storage.LoadTasks().FirstOrDefault(t => t.Id == id);
                if (task != null)
                {
                    _storage.DeleteTask(id);
                    _logger.Log($"Task deleted: '{task.Title}'");
                }
                else
                {
                    throw new Exception($"Task with ID {id} not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete task: {ex.Message}");
            }
        }
    }
}