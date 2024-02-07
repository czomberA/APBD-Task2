
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;

namespace Task2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var input = args[0];
            //var testInput = "Task2\\dane.csv";
            var output = args[1];
            //var testLog = "Task2\\uni.json";
            var logPath = args[2];
            //wrong number of arguments
            if (args.Length < 1 | args.Length > 4)
            {
                throw new ArgumentOutOfRangeException();
            }
            //data file errors
            
            if (!Directory.Exists(Path.GetDirectoryName(input)))
            {
                throw new ArgumentException("a path to a data file does not exist");
            }

            if (!File.Exists(args[0]))
            {

                throw new FileNotFoundException("The file does not exist");
            }

            //result errors
            string directory = Path.GetDirectoryName(args[2]);
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException("the result folder does not exist");
            }
            //log file errors
            if (!File.Exists(args[2]))
            {
                throw new FileNotFoundException("log file doesn't exist");
            }
            //format errors
            if (!args[3].Equals("json", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("format not supported");
            }
            
            Dictionary<string, int> activeStudies =
                new Dictionary<string, int>();
            var studentsSet = new HashSet<Student>(new StudentComparator());
            using var logWriter = new StreamWriter(logPath);
            using (StreamReader sr = new StreamReader(input))
            {
                while(sr.ReadLine() is { } currentLine)
                { 
                    string[] studentData = currentLine.Split(",");
                    
                    if (studentData.Length < 9)
                    {
                        logWriter.WriteLine($"The row does not have enough columns: {currentLine}");
                    }
                    else
                    {
                        Student student = new Student
                        {
                            index = studentData[4],
                            name = studentData[0],
                            surname = studentData[1],
                            birhdate = studentData[5],
                            email = studentData[6],
                            mothersName = studentData[7],
                            fathersName = studentData[8],
                            studies = new Studies
                            {
                                department = studentData[2],
                                mode = studentData[3],
                            },
                        };

                        bool valid = true;

                        for (int i = 0; i < studentData.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(studentData[i]))
                            {
                                logWriter.WriteLine($"The row cannot have empty columns: {currentLine}");
                                valid = false;
                            }
                        }

                        if (valid)
                        {
                            if (!studentsSet.Contains(student))
                            {
                                studentsSet.Add(student);
                                if (activeStudies.ContainsKey(student.studies.department))
                                {
                                    activeStudies[student.studies.department] += 1;
                                }
                                else
                                {
                                    activeStudies.Add(student.studies.department, 1);
                                }
                            }
                            else
                            {
                                logWriter.WriteLine($"Duplicate: {currentLine}");
                            }
                        }
                        

                        
                    }


                }
            }
            
            
            var studiesSet = new HashSet<ActiveStudies>();
            foreach (var active in activeStudies)
            {
                ActiveStudies act = new ActiveStudies
                {
                    name = active.Key,
                    numberOfStudents = active.Value

                };
                studiesSet.Add(act);
            }






            University university = new University
            {
                CreatedAt = DateTime.UtcNow.ToString("dd-MM-yyyy"), 
                Author = "Czomber", 
                Students = studentsSet,
                ActiveStudies = studiesSet
            };
            
            using(var writer = new StreamWriter(output))
            {
                var json = JsonConvert.SerializeObject(university, (Newtonsoft.Json.Formatting)Formatting.Indented);
                writer.Write(json);
            }
        }
            
    }

    
}