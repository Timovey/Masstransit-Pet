namespace StartService.Main
{
    public static class MatrixHelper
    {
        public static int[][] Transpon(int[][] arr)
        {
            int[][] res = new int[arr.Length][];
            for (int i = 0; i < arr.Length; i++)
            {
                res[i] = new int[arr.Length];
            }
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr.Length; j++)
                {
                    res[j][i] = arr[i][j];
                }
            }
            return res;
        }
    }
}
