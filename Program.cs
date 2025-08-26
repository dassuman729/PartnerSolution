using Newtonsoft.Json;

namespace PartnerSolution
{
    public class OpenTextTest
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("::::Triangle Display Test::::");
            // Call the fixed triangle function
            Console.WriteLine("\n\nFixed 3x4 Triangle:\n");
            TriangleDisplayTest.Display3X4Triangle();

            // Call the custom triangle function with example dimensions
            Console.WriteLine("\n\nCustom Triangle (M=5, N=3):\n");
            TriangleDisplayTest.DisplayCustomTriangle(5, 3);

            // Example with different dimensions
            Console.WriteLine("\n\nCustom Triangle (M=4, N=5):\n");
            TriangleDisplayTest.DisplayCustomTriangle(4, 5);

            Console.WriteLine("Press any key to continue to next test...");
            Console.ReadKey();
            Console.WriteLine("\n::::Partner and Solution Test::::\n");
            Console.WriteLine("Do you want to include partners that doesn't have any solution? (Y/N)\n");
            var key = Console.ReadKey();

            var partners = PartnerSolutionTest.GetPartners();
            var solutions = PartnerSolutionTest.GetSolutions();

            // Filter/Add partners without solutions
            if (key.KeyChar == 'y' || key.KeyChar == 'Y')
            {
                solutions.Where(s => !partners.Any(p => p.Name == s.Name)).ToList().ForEach(s =>
                {
                    partners.Add(new Partner
                    {
                        Name = s.PartnerName
                    });
                });
            }

            // Return only partners that have solutions
            var partnerSolutions = partners.GroupJoin(
                solutions,
                partner => partner.Name,
                solution => solution.PartnerName,
                (partner, solutionGroup) =>
                {
                    return new PartnerSolution
                    {
                        PartnerName = partner.Name,
                        SolutionName = string.Join(", ", solutionGroup.Select(s => s.Name))
                    };
                }).ToList();

            string jsonOutput = JsonConvert.SerializeObject(partnerSolutions, Formatting.Indented);
            Console.WriteLine(jsonOutput);

            Console.ReadKey();
        }
    }
}
