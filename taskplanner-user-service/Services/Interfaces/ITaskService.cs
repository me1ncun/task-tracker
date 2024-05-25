﻿namespace taskplanner_user_service.Services.Interfaces;

public interface ITaskService
{
    public Task Add(string title, string description, string status, int userId, DateTime doneAt);
    public Task<List<Models.Task>> GetByUserId(int id);
    public Task Update(int id, string title, string description, string status, DateTime doneAt);
    public Task Delete(int id);
}