﻿using Newtonsoft.Json.Linq;
using Syncfusion.Maui.AIAssistView;
using Syncfusion.Maui.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSchedulerAIAssistant
{
    public class SchedulerViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Holds the return message of AI.
        /// </summary>
        private string returnMessage = string.Empty;

        /// <summary>
        /// Collection of messages in a conversation.
        /// </summary>
        private ObservableCollection<object> messages;

        /// <summary>
        /// Holds the showHeader value.
        /// </summary>
        private bool showHeader = true;

        /// <summary>
        /// Holds the collection resources.
        /// </summary>
        private ObservableCollection<object>? resources;

        /// <summary>
        /// Holds the assist AI service.
        /// </summary>
        private AzureOpenAIService azureAIServices = new AzureOpenAIService();

        /// <summary>
        /// Used to handle the visibility of assist view.
        /// </summary>
        private bool showAssistView = false;

        /// <summary>
        /// Used to handle the visibility of activity indicator.
        /// </summary>
        private bool showIndicator = false;

        /// <summary>
        /// Holds appointment collection.
        /// </summary>
        private ObservableCollection<AppointmentModel>? appointmentCollection;

        /// <summary>
        /// Holds the Spphia start time collection.
        /// </summary>
        internal List<DateTime>? SophiaStartTimeCollection;

        /// <summary>
        /// Holds the Sophia end time collection.
        /// </summary>
        internal List<DateTime>? SophiaEndTimeCollection;

        /// <summary>
        /// Holds the Sophia subject collection.
        /// </summary>
        internal List<string>? SophiaSubjectCollection;

        /// <summary>
        /// Holds the Sophia location collection.
        /// </summary>
        internal List<string>? SophiaLocationCollection;

        /// <summary>
        /// Holds the Sophia resource ID collection.
        /// </summary>
        internal List<string>? SophiaResourceIDCollection;

        /// <summary>
        /// Holds the John start time collection.
        /// </summary>
        internal List<DateTime>? JohnStartTimeCollection;

        /// <summary>
        /// Holds the John end time collection.
        /// </summary>
        internal List<DateTime>? JohnEndTimeCollection;

        /// <summary>
        /// Holds the John subject collection.
        /// </summary>
        internal List<string>? JohnSubjectCollection;

        /// <summary>
        /// Holds the John location collection.
        /// </summary>
        internal List<string>? JohnLocationCollection;

        /// <summary>
        /// Holds the John resource ID collection.
        /// </summary>
        internal List<string>? JohnResourceIDCollection;

        /// <summary>
        /// Holds the Sophia time slots collection.
        /// </summary>
        internal List<string>? SophiaAvailableTimeSlots = new List<string>();

        /// <summary>
        /// Holds the John time slots collection.
        /// </summary>
        internal List<string>? JohnAvailableTimeSlots = new List<string>();

        /// <summary>
        /// Holds the current resource.
        /// </summary>
        internal string? CurrentResource;

        /// <summary>
        /// Holds the online appointment collection.
        /// </summary>
        public ObservableCollection<SchedulerAppointment>? Appointments = new ObservableCollection<SchedulerAppointment>();

        /// <summary>
        /// Gets or sets the collection of appointments.
        /// </summary>
        public ObservableCollection<AppointmentModel>? AppointmentCollection
        {
            get { return appointmentCollection; }
            set
            {
                this.appointmentCollection = value;
                this.RaisePropertyChanged("AppointmentCollection");
            }
        }

        /// <summary>
        /// Gets or sets the collection of messages of a conversation.
        /// </summary>
        public ObservableCollection<object> Messages
        {
            get
            {
                return this.messages;
            }

            set
            {
                this.messages = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection resources.
        /// </summary>
        public ObservableCollection<object>? Resources
        {
            get
            {
                return this.resources;
            }

            set
            {
                this.resources = value;
            }
        }

        /// <summary>
        /// Gets or sets the show header.
        /// </summary>
        public bool ShowHeader
        {
            get { return this.showHeader; }
            set { this.showHeader = value; RaisePropertyChanged("ShowHeader"); }
        }

        /// <summary>
        /// Gets or sets the show assist view.
        /// </summary>
        public bool ShowAssistView
        {
            get { return this.showAssistView; }
            set { this.showAssistView = value; RaisePropertyChanged("ShowAssistView"); }
        }

        /// <summary>
        /// Gets or sets the show activitity indicator.
        /// </summary>
        public bool ShowIndicator
        {
            get
            {
                return this.showIndicator;
            }
            set
            {
                this.showIndicator = value;
                RaisePropertyChanged("ShowIndicator");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerViewModel"/> class.
        /// </summary>
        public SchedulerViewModel()
        {
            this.messages = new ObservableCollection<object>();
            this.Resources = new ObservableCollection<object>();
            GenerateAppointmentInfo();
            InitializeResources();
        }

        /// <summary>
        /// Method to generate appointment collection.
        /// </summary>
        private void GenerateAppointmentInfo()
        {
            AppointmentCollection = new ObservableCollection<AppointmentModel>();
            AppointmentCollection.Add(new AppointmentModel() { Name = "Book an appointment with Dr. Sophia", ImageName = "people_circle1.png" });
            AppointmentCollection.Add(new AppointmentModel() { Name = "Book an appointment with Dr. John", ImageName = "people_circle8.png" });
        }

        /// <summary>
        /// Method to contain AI response and updates.
        /// </summary>
        /// <param name="userInput">The user input</param>
        /// <returns></returns>
        private async Task<string> GetRecommendation(string userInput)
        {
            DateTime todayDate = DateTime.Today;
            string prompt = $"Given data: {userInput}. Based on the given data, provide 10 appointment time details for Doctor1 and Doctor2 on {todayDate}." +
                            $"Availability time is 9AM to 6PM." +
                            $"In 10 appointments, split the time details as 5 for Doctor1 and 5 for Doctor2." +
                            $"Provide complete appointment time details for both Doctor1 and Doctor2 without missing any fields." +
                            $"It should be 30 minutes appointment duration." +
                            $"Doctor1 time details should not collide with Doctor2." +
                            $"Provide ResourceID for Doctor1 as 1000 and for Doctor2 as 1001." +
                            $"Do not repeat the same time. Generate the following fields: StartDate, EndDate, Subject, Location, and ResourceID." +
                            $"The return format should be the following JSON format: Doctor1[StartDate, EndDate, Subject, Location, ResourceID], Doctor2[StartDate, EndDate, Subject, Location, ResourceID]." +
                            $"Condition: provide details without any explanation. Don't include any special characters like ```";

            returnMessage = await azureAIServices.GetResponseFromGPT(prompt);
            var jsonObj = JObject.Parse(returnMessage);

            var doctorAppointments = new Dictionary<string, (List<DateTime> StartTimes, List<DateTime> EndTimes, List<string> Subjects, List<string> Locations, List<string> ResourceIDs)>
    {
        { "Doctor1", (new List<DateTime>(), new List<DateTime>(), new List<string>(), new List<string>(), new List<string>()) },
        { "Doctor2", (new List<DateTime>(), new List<DateTime>(), new List<string>(), new List<string>(), new List<string>()) }
    };

            foreach (var doctor in doctorAppointments.Keys)
            {
                foreach (var appointment in jsonObj[doctor]!)
                {
                    if (DateTime.TryParse((string)appointment["StartDate"]!, out DateTime startTime) && DateTime.TryParse((string)appointment["EndDate"]!, out DateTime endTime))
                    {
                        doctorAppointments[doctor].StartTimes.Add(startTime);
                        doctorAppointments[doctor].EndTimes.Add(endTime);
                    }
                    doctorAppointments[doctor].Subjects.Add((string)appointment["Subject"]!);
                    doctorAppointments[doctor].Locations.Add((string)appointment["Location"]!);
                    doctorAppointments[doctor].ResourceIDs.Add((string)appointment["ResourceID"]!);
                }
            }

            this.SophiaStartTimeCollection = doctorAppointments["Doctor1"].StartTimes;
            this.SophiaEndTimeCollection = doctorAppointments["Doctor1"].EndTimes;
            this.SophiaSubjectCollection = doctorAppointments["Doctor1"].Subjects;
            this.SophiaLocationCollection = doctorAppointments["Doctor1"].Locations;
            this.SophiaResourceIDCollection = doctorAppointments["Doctor1"].ResourceIDs;

            this.JohnStartTimeCollection = doctorAppointments["Doctor2"].StartTimes;
            this.JohnEndTimeCollection = doctorAppointments["Doctor2"].EndTimes;
            this.JohnSubjectCollection = doctorAppointments["Doctor2"].Subjects;
            this.JohnLocationCollection = doctorAppointments["Doctor2"].Locations;
            this.JohnResourceIDCollection = doctorAppointments["Doctor2"].ResourceIDs;

            FilterAppointmentsForSophia();
            FilterAppointmentsForJohn();

            this.SophiaAvailableTimeSlots = GenerateTimeSlots(SophiaStartTimeCollection);
            this.JohnAvailableTimeSlots = GenerateTimeSlots(JohnStartTimeCollection);

            return GenerateFinalTimeSlots(userInput);
        }

        /// <summary>
        /// Method to generate the time slots.
        /// </summary>
        /// <param name="timeCollection">The time collection</param>
        /// <returns></returns>
        private List<string> GenerateTimeSlots(List<DateTime> timeCollection)
        {
            return timeCollection.Select(time => time.ToString("hh:mm tt").ToUpper()).ToList();
        }

        /// <summary>
        /// Method to generate the final time slots.
        /// </summary>
        /// <param name="userInput">The user input</param>
        /// <returns></returns>
        private string GenerateFinalTimeSlots(string userInput)
        {
            string sophiaAvailedTimeSlots = string.Join(" \n ", this.SophiaAvailableTimeSlots!);
            string johnAvailedTimeSlots = string.Join(" \n ", this.JohnAvailableTimeSlots!);

            if (userInput.Contains("Sophia"))
            {
                return $"Doctor Sophia available appointment slots are\n {sophiaAvailedTimeSlots} \nEnter the time (hh:mm tt) to book an appointment.";
            }
            else if (userInput.Contains("John"))
            {
                return $"Doctor John available appointment slots are\n {johnAvailedTimeSlots} \nEnter the time (hh:mm tt) to book an appointment.";
            }
            else
            {
                return $"Doctor Sophia available appointment slots are\n {sophiaAvailedTimeSlots}\nDoctor John available appointment slots are\n {johnAvailedTimeSlots}\nEnter the time (hh:mm tt) to book an appointment.";
            }
        }

        /// <summary>
        /// Method to filter the Sophia appointments.
        /// </summary>
        private void FilterAppointmentsForSophia()
        {
            for (int i = 0; i < this.Appointments?.Count; i++)
            {
                if (SophiaStartTimeCollection!.Contains(this.Appointments[i].StartTime))
                {
                    SophiaStartTimeCollection.Remove(this.Appointments[i].StartTime);
                }
            }
        }

        /// <summary>
        /// Method to filter the John appointments.
        /// </summary>
        private void FilterAppointmentsForJohn()
        {
            for (int i = 0; i < this.Appointments?.Count; i++)
            {
                if (JohnStartTimeCollection!.Contains(this.Appointments[i].StartTime))
                {
                    JohnStartTimeCollection.Remove(this.Appointments[i].StartTime);
                }
            }
        }

        /// <summary>
        /// Method to get the AI response.
        /// </summary>
        /// <param name="query">The query</param>
        /// <returns></returns>
        public async Task GetAIResults(string query)
        {
            await Task.Delay(1000).ConfigureAwait(true);
            var reply = await this.GetRecommendation(query);
            AssistItem botMessage = new AssistItem() { Text = reply, ShowAssistItemFooter = false };
            this.Messages.Add(botMessage);
        }

        /// <summary>
        /// Method to initialize the resources.
        /// </summary>
        private void InitializeResources()
        {
            for (int i = 0; i < 2; i++)
            {
                ResourceViewModel resourceViewModel = new ResourceViewModel();

                if (i == 0)
                {
                    resourceViewModel.Name = "Sophia";
                    resourceViewModel.ImageName = "people_circle1.png";
                    resourceViewModel.Id = "1000";
                    resourceViewModel.Background = new SolidColorBrush(Color.FromArgb("#36B37B"));
                }
                else
                {
                    resourceViewModel.Name = "John";
                    resourceViewModel.ImageName = "people_circle8.png";
                    resourceViewModel.Id = "1001";
                    resourceViewModel.Background = new SolidColorBrush(Color.FromArgb("#8B1FA9"));
                }

                Resources?.Add(resourceViewModel);
            }
        }


        #region PropertyChanged

        /// <summary>
        /// Property changed handler.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Occurs when property is changed.
        /// </summary>
        /// <param name="propName">changed property name</param>
        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion
    }

}
