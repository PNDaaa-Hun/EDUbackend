namespace EDUBackEnd.Services.User
{
    public class StudentIdGenerator
    {
        private static readonly char[] chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        public static string GenerateId()
        {
            Random random = new Random();
            char[] id = new char[6];

            for (int i = 0; i < 6; i++)
            {
                id[i] = chars[random.Next(chars.Length)];
            }

            return new string(id);
        }
    }
}
