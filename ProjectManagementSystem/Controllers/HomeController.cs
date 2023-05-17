using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ProjectManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<ApplicationUser> userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            // 检查用户是否已登录
            if (!User.Identity.IsAuthenticated)
            {
                return View(); // 或者返回其他视图，比如登录页面
            }

            var model = new HomeIndexViewModel();



            return View(model);
        }

        public IActionResult ReturnToIndex()
        {
            return RedirectToAction("Index");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


        public async Task<IActionResult> AddData()
        {
            var init = new DbInit(applicationDbContext);
            await init.Init();
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> GetBarData()
        {
            var projects = await applicationDbContext.Projects
                .Include(p => p.Missions)
                .ToListAsync();
            var categories = projects.Select(p => p.Name).ToList();
            var undealData = projects
                .Select(project => project.Missions.Count(mission => mission.CreateDate > DateTime.Now)).ToList();
            var inprogressData = projects
                .Select(project => project.Missions.Count(mission => mission.StartDate < DateTime.Now && mission.FinishedTime > DateTime.Now)).ToList();
            var finishedData = projects
                .Select(project => project.Missions.Count(mission => mission.FinishedTime < DateTime.Now)).ToList();

            var data = new
            {
                categories = categories,
                series = new[]
                {
                    new
                    {
                        name="已完成",
                        color = "#ff771d",
                        data= finishedData,
                    },
                    new
                    {
                        name="进行中",
                        color = "#2eca6a",
                        data= inprogressData,
                    },
                    new
                    {
                        name="待处理",
                        color = "#4154f1",
                        data= undealData,
                    },
                },
            };
            return Json(data);
        }

    }

    public class DbInit
    {
        private readonly int projectCount = 5;

        private readonly int userCount = 10;
        /// <summary>
        /// 项目中的人员数
        /// </summary>
        private readonly int projectUserCount = 8;




        private ApplicationDbContext applicationDbContext;
        public DbInit(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task Init()
        {
            await AddData();
        }
        public async Task AddData()
        {
            await AddDepartment();
            await AddPosition();
            await AddUser();
            for (int i = 0; i < projectCount; i++)
            {
                await AddProject($"项目{i + 1}");
                await AddFundAndChanges($"项目{i + 1}");
                await AddMissionAndExcutor($"项目{i + 1}");
            }
        }

        private async Task AddDepartment()
        {
            var d1 = new Department
            {
                Name = "设计部",
            };
            await applicationDbContext.Departments.AddAsync(d1);
            var d2 = new Department
            {
                Name = "行政部",
            };
            await applicationDbContext.Departments.AddAsync(d2);
            var d3 = new Department
            {
                Name = "研发部",
            };
            await applicationDbContext.Departments.AddAsync(d3);
            var d4 = new Department
            {
                Name = "财务部",
            };
            await applicationDbContext.Departments.AddAsync(d4);
            await applicationDbContext.SaveChangesAsync();
        }

        private async Task AddPosition()
        {
            var positions = new List<Position>
            {
                new Position { Name = "设计总监",},
                new Position { Name="艺术总监",},
                new Position { Name= "插画师",},

                new Position { Name = "行政总监",},
                new Position { Name="人事专员",},
                new Position { Name= "行政文员",},

                new Position { Name = "研发总监",},
                new Position { Name="研发工程师",},
                new Position { Name= "测试工程师",},

                new Position { Name = "财务总监",},
                new Position { Name="成本会计师",},
                new Position { Name= "审计经理",},
            };
            await applicationDbContext.Positions.AddRangeAsync(positions);
            await applicationDbContext.SaveChangesAsync();
        }

        private async Task AddUser()
        {
            var departments = await applicationDbContext.Departments.ToListAsync();
            var positions = await applicationDbContext.Positions.ToListAsync();
            var users = new List<ApplicationUser>();
            for (int i = 0; i < userCount; i++)
            {
                Random derandom = new Random();
                var dIndex = derandom.Next(departments.Count - 1);
                Random porandom = new Random();
                var pIndex = porandom.Next(positions.Count - 1);
                var user = new ApplicationUser
                {
                    UserName = $"人员{i + 1}",
                    TrueName = $"人员{i + 1}",
                    Department = departments[dIndex],
                    Job = positions[pIndex],
                };
                users.Add(user);
            }
            await applicationDbContext.ApplicationUsers.AddRangeAsync(users);
            await applicationDbContext.SaveChangesAsync();
        }

        private async Task AddProject(string projectName)
        {
            var users = await applicationDbContext.ApplicationUsers
                .ToListAsync();
            var usersInProject = users.GetRange(0, Math.Min(projectUserCount, users.Count));
            Random random = new Random();
            var putUserIndex = random.Next(usersInProject.Count - 1);
            var funUserIndex = random.Next(usersInProject.Count - 1);
            var project = new Project
            {
                Name = projectName,
                Description = "项目的描述",
                CreateDate = new DateTime(2023, 5, 21, 10, 25, 55),
                Deadline = new DateTime(2023, 11, 28, 10, 25, 55),
                Status = ProjectStatus.进行中,
                PutForward = usersInProject[putUserIndex],
                Functionary = usersInProject[funUserIndex],
                ProjectUsers = new List<ProjectUser>(),
            };
            foreach (var user in usersInProject)
            {
                project.ProjectUsers.Add(new ProjectUser { ApplicationUser = user, Project = project });
            }
            await applicationDbContext.Projects.AddAsync(project);
            await applicationDbContext.SaveChangesAsync();
        }

        private async Task AddFundAndChanges(string projectName)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.ApplicationUser)
                .FirstOrDefaultAsync(p => p.Name == projectName);
            var usersInThisProject = project.ProjectUsers
                .Select(pu => pu.ApplicationUser)
                .ToList();
            var fund = new Fund
            {
                Project = project,
                Amount = 1000000,
            };
            await applicationDbContext.Funds.AddAsync(fund);
            await applicationDbContext.SaveChangesAsync();
            var random = new Random();
            var fundChanges = new List<FundChange>();
            var fundChangeCount = random.Next(100, 300);
            for (int i = 0; i < fundChangeCount; i++)
            {
                bool randomBool = random.Next(2) == 0;
                int randomNumber = random.Next(1, 10000);
                var userIndex = random.Next(0, usersInThisProject.Count - 1);
                var fundChange = new FundChange
                {
                    Number = randomNumber,
                    DateTime = GetRandomTime(),
                    ChangeType = randomBool ? FundChangeType.支出 : FundChangeType.收入,
                    Fund = fund,
                    Description = $"原因{i}",
                    User = usersInThisProject[userIndex],
                };
                fundChanges.Add(fundChange);
            }
            await applicationDbContext.FundChanges.AddRangeAsync(fundChanges);
            await applicationDbContext.SaveChangesAsync();
        }

        private async Task AddMissionAndExcutor(string projectName)
        {
            var project = await applicationDbContext.Projects
                .Include(p => p.ProjectUsers)
                .ThenInclude(pu => pu.ApplicationUser)
               .FirstOrDefaultAsync(p => p.Name == projectName);
            var usersInThiProject = project.ProjectUsers
                .Select(pu => pu.ApplicationUser)
                .ToList();
            var random = new Random();
            var missions = new List<Mission>();
            var ProjectMissionCount = random.Next(100, 500);
            for (int i = 0; i < ProjectMissionCount; i++)
            {
                var time = GetRandomTime();
                var timeToStart = random.Next(1, 40);
                var startTime = time.AddDays(timeToStart);
                var timeToFinsh = random.Next(1, 40);
                var finishTime = startTime.AddDays(timeToFinsh);
                var priorityInt = random.Next(0, 3);
                var missionStatus = MissionStatus.进行中;
                if (DateTime.Now > finishTime)
                    missionStatus = MissionStatus.已完成;
                if (DateTime.Now < startTime)
                    missionStatus = MissionStatus.待处理;
                var putUserIndex = random.Next(0, usersInThiProject.Count - 1);
                var mission = new Mission
                {
                    Name = $"任务{i}",
                    Description = $"任务{i}的描述",
                    CreateDate = time,
                    StartDate = startTime,
                    FinishedTime = finishTime,
                    Deadline = finishTime,
                    Priority = (MissionPriority)priorityInt,
                    Status = missionStatus,
                    Project = project,
                    PutForward = usersInThiProject[putUserIndex],
                    MissionExecutors = new List<MissionExecutor>(),
                };

                var countRandom = random.Next(1, 3);
                bool[] visited = new bool[usersInThiProject.Count];
                for (int j = 0; j < countRandom; j++)
                {
                    var exRandom = random.Next(0, usersInThiProject.Count - 1);
                    if (visited[exRandom]) continue;
                    var me = new MissionExecutor
                    {
                        Mission = mission,
                        ApplicationUser = usersInThiProject[exRandom],
                    };
                    mission.MissionExecutors.Add(me);
                    visited[exRandom] = true;
                }
                missions.Add(mission);
            }
            await applicationDbContext.Missions.AddRangeAsync(missions);
            await applicationDbContext.SaveChangesAsync();
        }





        public DateTime GetRandomTime()
        {
            Random random = new Random();

            // 生成随机的年份
            int year = 2023;

            // 生成随机的月份
            int month = random.Next(1, 12);

            // 生成随机的日期
            int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);

            // 生成随机的小时
            int hour = random.Next(0, 24);

            // 生成随机的分钟
            int minute = random.Next(0, 60);

            // 生成随机的秒钟
            int second = random.Next(0, 60);

            DateTime randomDateTime = new DateTime(year, month, day, hour, minute, second);

            if (randomDateTime > DateTime.Now.AddDays(100))
                return GetRandomTime();
            return randomDateTime;
        }

    }
}