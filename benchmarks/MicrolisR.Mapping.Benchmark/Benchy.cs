using AutoMapper;
using BenchmarkDotNet.Attributes;
using MicrolisR.Mapping.Benchmark.Models;

namespace MicrolisR.Mapping.Benchmark;



[MemoryDiagnoser]
public class Benchy
{
    private static readonly MicrolisR.Mapping.Abstractions.IMapper Mapper =
        new Mapper(typeof(Employee), typeof(EmployeeDbo));
    
    private static readonly MapperConfiguration Config = new(cfg =>
        {
            cfg.CreateMap<Employee, EmployeeDbo>();
            cfg.CreateMap<EmployeeDbo, Employee>();
            cfg.CreateMap<Job, JobDbo>();
            cfg.CreateMap<JobDbo, Job>();
        }
    );
    
    private static readonly IMapper AutoMapper = new AutoMapper.Mapper(Config);
    
    public static readonly Employee TestClass = new()
    {
        Firstname = "Tom",
        Lastname = "Holland",
        Job = new Job()
        {
            Id = Guid.NewGuid(),
            Titel = "Painter"
        }
    };
    
    private static readonly EmployeeDbo TestClassDto = new()
    {
        Id = 4712,
        Firstname = "Tom",
        Lastname = "Holland",
        Job = new JobDbo()
        {
            Id = 4711,
            JobId = Guid.NewGuid(),
            Titel = "Painter",
            CreatedAt = DateTimeOffset.UtcNow
        }
    };
    
    [Benchmark]
    public EmployeeDbo Manuel_Mapping()
    {
        return Map(TestClass);
    }
    
    private EmployeeDbo Map(Employee employee)
    {
        return new EmployeeDbo()
        { 
            Firstname = employee.Firstname,
            Lastname = employee.Lastname,
            Job = new JobDbo()
            {
                JobId = employee.Job.Id,
                Titel = employee.Job.Titel,
            }
        };
    }
    
    private Employee Map(EmployeeDbo employee)
    {
        return new Employee()
        { 
            Firstname = employee.Firstname,
            Lastname = employee.Lastname,
            Job = new Job()
            {
                Id = employee.Job.JobId,
                Titel = employee.Job.Titel,
            }
        };
    }
    
    [Benchmark]
    public Employee Manuel_Mapping_Back()
    {
        return Map(TestClassDto);
    }
    
    
    // [Benchmark]
    // public EmployeeDbo AutoMapper_Mapping()
    // {
    //     var dto = AutoMapper.Map<EmployeeDbo>(TestClass);
    //     return dto;
    // }
    
    
    // [Benchmark]
    // public Employee AutoMapper_Mapping_Back()
    // {
    //     var dto = AutoMapper.Map<Employee>(TestClassDto);
    //
    //     return dto;
    // }
    

    
    [Benchmark]
    public EmployeeDbo? MicrolisR_Mapping()
    {
        var dto = Mapper.Map<EmployeeDbo>(TestClass);
        return dto;
    }
    
    [Benchmark]
    public Employee? MicrolisR_Mapping_Back()
    {
        var dto = Mapper.Map<Employee>(TestClassDto);
        return dto;
    }
    
    [Benchmark]
    public EmployeeDbo? MicrolisR_Mapping_Explicit()
    {
        var dto = Mapper.Map<Employee, EmployeeDbo>(TestClass);
        return dto;
    }
    
    [Benchmark]
    public Employee? MicrolisR_Mapping_Explicit_Back()
    {
        var dto = Mapper.Map<EmployeeDbo, Employee>(TestClassDto);
        return dto;
    }
}