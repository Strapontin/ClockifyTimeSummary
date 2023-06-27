using Clockify.Net;
using System.Text.RegularExpressions;

namespace ClockifyAPI
{
    public partial class Form1 : Form
    {
        private readonly string API_KEY;
        private const string CELL_PROJECT = "Project";
        private const string CELL_TOTAL_TIME = "TotalTime";
        private const string CELL_INTERVAL_TIME = "IntervalTime";
        private const string CELL_TASK = "Task";

        public Form1()
        {
            API_KEY = File.ReadAllText("./ClockifyApiKey.txt");
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            SetDefaultTime();
            await GetTimes();
            button1.Enabled = true;
        }

        private void SetDefaultTime()
        {
            int diff = (7 + (DateTime.Now.DayOfWeek - DayOfWeek.Sunday)) % 7;
            var date = DateTime.Now.AddDays(-1 * diff).Date;
            txtStartingDate.Text = date.ToString("dd/MM/yyyy");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            await GetTimes();
            button1.Enabled = true;
        }

        private async Task GetTimes()
        {
            textBox1.Text = "";
            dataGridView1.Rows.Clear();

            if (!DateTimeOffset.TryParse(txtStartingDate.Text, out var date))
            {
                MessageBox.Show("La date n'est pas reconnue");
                return;
            }

            List<DisplayedTime> displayedTimes = new();

            var clockify = new ClockifyClient(API_KEY);
            var workspace = (await clockify.GetWorkspacesAsync()).Data;
            var workspaceId = workspace.First().Id;
            var projects = (await clockify.FindAllProjectsOnWorkspaceAsync(workspaceId)).Data;

            // Récupère les noms de toutes les tâches
            List<Clockify.Net.Models.Tasks.TaskDto> taskDtos = new();
            foreach (var project in projects)
            {
                var tasksInProject = (await clockify.FindAllTasksAsync(workspaceId, project.Id, pageSize: 500)).Data;
                taskDtos.AddRange(tasksInProject);
            }


            var userId = (await clockify.GetCurrentUserAsync()).Data.Id;

            var allTimeEntries = (await clockify.FindAllTimeEntriesForUserAsync(workspaceId, userId, start: date)).Data;
            var grouppedTE = allTimeEntries.GroupBy(te => te.TaskId);

            var grouppedTEWithoutTask = grouppedTE.FirstOrDefault(gte => gte.Key == null);

            // Si on a un élément avec la clé à null, c'est qu'il n'a pas de task d'assignées
            if (grouppedTEWithoutTask.Any())
            {
                // On groupe les éléments sur la description, en prenant uniquement cette semaine
                foreach (var item in grouppedTEWithoutTask.GroupBy(x => x.Description))
                {
                    string projectName = projects.First(p => p.Id == item.First().ProjectId).Name;
                    var displayedTime = new DisplayedTime(item.Key, GetTimeFromClockifyDuration(item.Select(i => i.TimeInterval.Duration)));
                    displayedTime.ProjectName = projectName;

                    displayedTimes.Add(displayedTime);

                    AddToDataRow(displayedTime);
                }
            }

            // On récupère les durées totales de toutes les tâches
            foreach (var item in grouppedTE.Where(gte => gte.Key != null))
            {
                string taskId = item.First().TaskId;
                var timeEntries = (await clockify.FindAllTimeEntriesForUserAsync(workspaceId, userId, task: item.First().TaskId)).Data;

                DisplayedTime displayedTime = new();
                displayedTimes.Add(displayedTime);

                foreach (var timeEntry in timeEntries)
                {
                    displayedTime.TaskName = taskDtos.First(t => t.Id == timeEntry.TaskId).Name;
                    displayedTime.ProjectName = projects.First(p => p.Id == timeEntry.ProjectId).Name;

                    displayedTime.Description = SetName(displayedTime.TaskName, displayedTime.ProjectName);
                    displayedTime.AddTime(GetTimeFromClockifyDuration(timeEntry.TimeInterval.Duration));
                }

                AddToDataRow(displayedTime);
            }

            var displayedValues = displayedTimes.OrderBy(dt => dt.Description).Select(dt => $"{dt.TotalTime} -- {dt.Description}");


            textBox1.Text = String.Join(Environment.NewLine, displayedValues);
        }

        private string SetName(string taskName, string projectName)
        {
            var result = $"({projectName}) ~~ {taskName}";
            return result;
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

        private void AddToDataRow(DisplayedTime displayedTime)
        {
            var row = dataGridView1.Rows[dataGridView1.Rows.Add()];
            row.Cells[CELL_PROJECT].Value = displayedTime.ProjectName;
            row.Cells[CELL_TOTAL_TIME].Value = displayedTime.TotalTime;
            //row.Cells[CELL_INTERVAL_TIME] = displayedTime.;
            row.Cells[CELL_TASK].Value = displayedTime.TaskName;
        }

        private TimeSpan GetTimeFromClockifyDuration(string duration)
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