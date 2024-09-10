using Azure.AI.OpenAI;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;

namespace MauiSchedulerAIAssistant
{
    internal class AzureOpenAIService
    {
        const string endpoint = "https://{YOUR_END_POINT}.openai.azure.com";
        const string deploymentName = "GPT35Turbo";
        string key = "API key";

        OpenAIClient? client;
        ChatCompletionsOptions? chatCompletions;

        internal AzureOpenAIService()
        {

        }

        /// <summary>
        /// Get or Set Azure OpenAI client.
        /// </summary>
        public OpenAIClient? Client
        {
            get
            {
                return client;
            }
            set
            {
                client = value;
            }
        }

        internal async Task<string> GetResponseFromGPT(string userPrompt)
        {
            this.chatCompletions = new ChatCompletionsOptions
            {
                DeploymentName = deploymentName,
                Temperature = (float)0.5,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };

            this.client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
            if (this.client != null)
            {
                // Add the user's prompt as a user message to the conversation.
                this.chatCompletions?.Messages.Add(new ChatRequestSystemMessage("You are a predictive analytics assistant."));

                // Add the user's prompt as a user message to the conversation.
                this.chatCompletions?.Messages.Add(new ChatRequestUserMessage(userPrompt));
                try
                {
                    // Send the chat completion request to the OpenAI API and await the response.
                    var response = await this.client.GetChatCompletionsAsync(this.chatCompletions);

                    // Return the content of the first choice in the response, which contains the AI's answer.
                    return response.Value.Choices[0].Message.Content;
                }
                catch
                {
                    // If an exception occurs (e.g., network issues, API errors), return an empty string.
                    return "";
                }
            }

            return "";
        }
    }


    internal static class Utils
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }

    public class SfImageSourceConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>

        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
        {
            string? source = value as string;
            string? assemblyName = typeof(SfImageSourceConverter).GetTypeInfo().Assembly.GetName().Name; //GetType().GetTypeInfo().Assembly.GetName().Name;
            return ImageSource.FromResource(assemblyName + ".Resources.Images." + source, typeof(SfImageSourceConverter).GetTypeInfo().Assembly);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
