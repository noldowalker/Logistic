﻿using Domain.Models;

namespace Domain.WorkResults;

public interface IActionResult<T> where T : BaseModel
{
    public List<T> Data { get; init; }
    public List<ActionMessage> Messages { get; init; }
    public bool IsSuccessful { get; init; }
}