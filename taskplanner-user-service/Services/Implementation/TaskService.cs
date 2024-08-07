﻿using AutoMapper;
using taskplanner_user_service.DTOs;
using taskplanner_user_service.Exceptions;
using taskplanner_user_service.Helpers;
using taskplanner_user_service.Repositories.Interfaces;
using taskplanner_user_service.Services.Interfaces;
using Task = taskplanner_user_service.Models.Task;

namespace taskplanner_user_service.Services.Implementation;

public class TaskService: ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    
    public TaskService(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }
    
    public async Task<AddTaskResponse> Add(AddTaskRequest request)
    {
        var task = _mapper.Map<Models.Task>(request);
        
        task.DoneAt = DateHelper.GetDateTimeByStatus(request.Status);
        
        await _taskRepository.InsertAsync(task);
        
        var response = _mapper.Map<AddTaskResponse>(task);
        
        return response;
    }
    
    public async Task<List<GetTaskResponse>> GetTasksByUserId(GetTaskRequest request)
    {
        var tasks = await _taskRepository.GetByUserIdAsync(request.Id);
        
        var tasksDto = _mapper.Map<List<GetTaskResponse>>(tasks);
        
        return tasksDto;
    }

    public async Task<UpdateTaskResponse> Update(UpdateTaskRequest request)
    {
        var taskExist = await _taskRepository.GetByIdAsync(request.Id);
        if (taskExist is null)
        {
            throw new NotFoundException();
        }

        var task = _mapper.Map<Task>(request);
        task.DoneAt = DateHelper.GetDateTimeByStatus(request.Status);
        
        await _taskRepository.UpdateAsync(task.Id, task.Title, task.Description, task.Status, task.DoneAt);
        
        var response = _mapper.Map<UpdateTaskResponse>(task);
        
        return response;
    }
    
    public async Task<DeleteTaskResponse> Delete(DeleteTaskRequest request)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);
        if (task is null)
        {
            throw new NotFoundException();
        }
        if(task.UserId != request.UserId)
        {
            throw new UnauthorizedException();
        }
        
        await _taskRepository.DeleteAsync(task.Id);
        
        var response = _mapper.Map<DeleteTaskResponse>(task);
        
        return response;
    }
}