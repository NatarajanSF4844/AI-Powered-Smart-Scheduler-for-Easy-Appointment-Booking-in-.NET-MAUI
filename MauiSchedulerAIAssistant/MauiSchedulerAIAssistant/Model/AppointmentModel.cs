using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiSchedulerAIAssistant
{
    public class AppointmentModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentModel"/> class.
        /// </summary>
        public AppointmentModel()
        {
            this.Name = string.Empty;
            this.ImageName = string.Empty;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the image name.
        /// </summary>
        public string ImageName { get; set; }
    }
}
