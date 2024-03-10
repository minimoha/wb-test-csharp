namespace TestApi.Models;
public class State
    {
        public string state { get; set; }
        public string alias { get; set; }
        public List<string> lgas { get; set; }
    }

    public class GetStates
    {
        public static List<State> LoadStates()
        {
            List<State> items = new List<State>();

            var filePath = Path.Combine("Resources", "states.json");

            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<State>>(json);
            }
            return items;
        }
        public static void Main1()
        {
            string filePath = @"Utilities/states.json";

            try
            {
                string jsonString = File.ReadAllText(filePath);

                var data = System.Text.Json.JsonSerializer.Deserialize<List<State>>(jsonString);

                Console.WriteLine($"Value from JSON: {data}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static bool LgaExists(List<string> names, string searchName)
        {
            foreach (string name in names)
            {
                if (string.Equals(name, searchName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }

