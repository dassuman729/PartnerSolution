namespace PartnerSolution
{
    public static class TriangleDisplayTest
    {
        // Part A: Displays a fixed 3x4 triangle
        // Assumption: The triangle is right-aligned and displays '*' characters.
        // 3 is the number of rows, and 4 is the number of stars added per row.
        public static void Display3X4Triangle()
        {
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= i * 4; j++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
        }

        // Assumption: The triangle is right-aligned and displays '*' characters.
        // M is the number of rows, and N is the number of stars added per row.
        public static void DisplayCustomTriangle(int m, int n)
        {
            if (m <= 0 || n <= 0)
            {
                Console.WriteLine("Error: M and N must be positive integers.");
                return;
            }

            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= i * n; j++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
        }
    }
}