using Clockify.Net;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClockifyAPI
{
    public partial class FormDisplayTimes : Form
    {
        private readonly string API_KEY;
        private readonly ClockifyClient clockifyClient;

        private const string CELL_PROJECT = "Project";
        private const string CELL_TOTAL_TIME = "TotalTime";
        private const string CELL_INTERVAL_TIME = "TimeSinceStartDate";
        private const string CELL_TASK = "Task";

        public FormDisplayTimes()
        {
            API_KEY = File.ReadAllText("./ClockifyApiKey.txt");
            clockifyClient = new(API_KEY);

            InitializeComponent();
        }

        private async void FormDisplayTimes_Load(object sender, EventArgs e)
        {
            this.Text = $"Affichage des temps de : {(await clockifyClient.GetCurrentUserAsync()).Data.Name}";
            SetDefaultTime();
            await GetTimes();
            btnRefresh.Enabled = true;
        }

        private void SetDefaultTime()
        {
            int diff = (7 + (DateTime.Now.DayOfWeek - DayOfWeek.Sunday)) % 7;
            var date = DateTime.Now.AddDays(-1 * diff).Date;
            txtStartingDate.Text = date.ToString("dd/MM/yyyy");
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            await GetTimes();
            btnRefresh.Enabled = true;
        }

        private async Task GetTimes()
        {
            dgvClockifyTimes.Rows.Clear();

            if (!DateTimeOffset.TryParse(txtStartingDate.Text, out var date))
            {
                MessageBox.Show("La date n'est pas reconnue");
                return;
            }

            var userId = (await clockifyClient.GetCurrentUserAsync()).Data.Id;
            var workspace = (await clockifyClient.GetWorkspacesAsync()).Data;
            var workspaceId = workspace.First().Id;
            var projects = (await clockifyClient.FindAllProjectsOnWorkspaceAsync(workspaceId)).Data;

            // Récupère les noms de toutes les tâches
            List<Clockify.Net.Models.Tasks.TaskDto> allTasks = new();
            foreach (var project in projects)
            {
                var tasksInProject = (await clockifyClient.FindAllTasksAsync(workspaceId, project.Id, pageSize: 500)).Data;
                allTasks.AddRange(tasksInProject);
            }


            var allTimeEntriesAfterDate = (await clockifyClient.FindAllTimeEntriesForUserAsync(workspaceId, userId, start: date, pageSize: 500)).Data;
            var grouppedTimeEntriesAfterDate = allTimeEntriesAfterDate.GroupBy(te => te.TaskId);

            var grouppedTimeEntriesAfterDateWithoutTask = grouppedTimeEntriesAfterDate.FirstOrDefault(gte => gte.Key == null);

            // Si on a un élément avec la clé à null, c'est qu'il n'a pas de task d'assignées
            if (grouppedTimeEntriesAfterDateWithoutTask.Any())
            {
                // On groupe les éléments sur la description, en prenant uniquement cette semaine
                foreach (var item in grouppedTimeEntriesAfterDateWithoutTask.GroupBy(x => x.Description))
                {
                    string projectName = projects.First(p => p.Id == item.First().ProjectId).Name;
                    var time = GetTimeFromClockifyDuration(item.Select(i => i.TimeInterval.Duration));
                    var displayedTime = new DisplayedTime(time)
                    {
                        ProjectName = projectName,
                        IntervalTime = time,
                    };

                    AddToDataRow(displayedTime);
                }
            }

            // On récupère les durées totales de toutes les tâches
            foreach (var item in grouppedTimeEntriesAfterDate.Where(gte => gte.Key != null))
            {
                string taskId = item.First().TaskId;
                var fullTimeEntries = (await clockifyClient.FindAllTimeEntriesForUserAsync(workspaceId, userId, task: item.First().TaskId)).Data!;

                var totalTime = GetTimeFromClockifyDuration(fullTimeEntries.Select(i => i.TimeInterval.Duration));
                var timeSinceStartDate = GetTimeFromClockifyDuration(item.Select(i => i.TimeInterval.Duration));

                DisplayedTime displayedTime = new(totalTime)
                {
                    TaskName = allTasks.First(t => t.Id == fullTimeEntries[0].TaskId).Name,
                    ProjectName = projects.First(p => p.Id == fullTimeEntries[0].ProjectId).Name,
                    IntervalTime = timeSinceStartDate,
                };

                AddToDataRow(displayedTime);
            }
        }

        private void AddToDataRow(DisplayedTime displayedTime)
        {
            var row = dgvClockifyTimes.Rows[dgvClockifyTimes.Rows.Add()];
            row.Cells[CELL_PROJECT].Value = displayedTime.ProjectName;
            row.Cells[CELL_TOTAL_TIME].Value = displayedTime.TotalTime;
            row.Cells[CELL_INTERVAL_TIME].Value = displayedTime.IntervalTime;
            row.Cells[CELL_TASK].Value = displayedTime.TaskName;
        }

        private TimeSpan GetTimeFromClockifyDuration(IEnumerable<string> durations)
        {
            TimeSpan result = new();

            foreach (var duration in durations)
            {
                result = result.Add(GetTimeFromClockifyDuration(duration));
            }

            return result;
        }

        private static TimeSpan GetTimeFromClockifyDuration(string duration)
        {
            duration = duration.Remove(0, 2);

            string regularExpression = @"(?<hours>\d+H)?(?<minutes>\d+M)?(?<seconds>\d+S)?";
            Regex regex = new(regularExpression);
            var groups = regex.Match(duration).Groups;

            int.TryParse(groups["hours"].ToString().TrimEnd('H'), out int hours);
            int.TryParse(groups["minutes"].ToString().TrimEnd('M'), out int minutes);
            int.TryParse(groups["seconds"].ToString().TrimEnd('S'), out int seconds);

            return new TimeSpan(hours, minutes, seconds);
        }
    }
}