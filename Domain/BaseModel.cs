using System;

namespace Domain
{
  public abstract class BaseModel
  {
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
  }
}